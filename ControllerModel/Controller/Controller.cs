using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ControllerModel.Controller
{
    /// <summary>
    /// Класс регулятора
    /// </summary>
    public class Controller
    {
        private static Controller _instance;

        /// <summary>
        /// Сущность регулятора
        /// </summary>
        public static Controller Instance {
            get {
                Controller.Init();
                return Controller._instance;
            }
        }

        /// <summary>
        /// Инициализация сущности регулятора
        /// </summary>
        public static void Init()
        {
            if (Controller._instance == null)
            {
                Controller._instance = new Controller();
            }
        }

        /// <summary>
        /// Период квантования
        /// </summary>
        public const int QUANTIZATION_PERIOD = 100;

        private bool IsRunning { get; set; } = false;

        /// <summary>
        /// Целевое значение температуры газа
        /// </summary>
        public double TargetT { get; set; } = 0;

        /// <summary>
        /// Текущая температура газа на объекте
        /// </summary>
        public double CurrentT { get; set; } = 0;

        private double _currentG = 1;

        /// <summary>
        /// Текущий объем поступаемого воздуха
        /// </summary>
        public double CurrentG {
            get { return _currentG; }
            set {
                PreviousG = CurrentG;
                _currentG = value;
            }
        }

        /// <summary>
        /// Значение объёма поступаемого воздуха в предыдущий квант времени
        /// </summary>
        public double PreviousG { get; set; } = 1;

        private double _currentControllingError = 0;

        /// <summary>
        /// Ошибка регулирования
        /// </summary>
        public double CurrentControllingError {
            get {
                return _currentControllingError;
            }
            set {
                PreviousControllingError = CurrentControllingError;
                _currentControllingError = value;
            }
        }

        /// <summary>
        /// Предыдущее значение ошибки регулирования
        /// </summary>
        public double PreviousControllingError { get; set; } = 0;

        /// <summary>
        /// Подключение к объекту
        /// </summary>
        public ObjectConnection ConnectionHandler { get; set; }

        private Controller()
        {
            ConnectionHandler = new ObjectConnection();
        }

        /// <summary>
        /// Движок регулятора
        /// </summary>
        public void Go()
        {
            CurrentControllingError = TargetT - CurrentT;
            double newG = HandleError(CurrentControllingError);
            Console.WriteLine(
                $"Control: CurError={CurrentControllingError}; PrevError={PreviousControllingError}; PreviousG={PreviousG}; OutputG={newG}"
            );
            ConnectionHandler.SendG(newG);
        }

        /// <summary>
        /// Функция регулирования ошибки
        /// </summary>
        /// <param name="error">Ошибка регулирования</param>
        /// <returns>Объем поступающего воздуха</returns>
        public double HandleError(double error)
        {
            return PreviousG + (49.05 * CurrentControllingError) - (48.39 * PreviousControllingError);
        }
    }

    /// <summary>
    /// Подключение к объекту
    /// </summary>
    public class ObjectConnection
    {
        /// <summary>
        /// URL-адрес симуляции объекта управления
        /// </summary>
        public const string OBJECT_URL = "http://localhost:54467/hub";

        public HubConnection Connection { get; }

        /// <summary>
        /// Конструктор. 
        /// Создание соединения.
        /// </summary>
        public ObjectConnection()
        {
            Connection = new HubConnectionBuilder()
                .WithUrl(OBJECT_URL)
                .Build();
            Connection.StartAsync();
            Connection.On("Meter", (double T, double G) =>
            {
                Controller.Instance.CurrentT = T;
                Controller.Instance.CurrentG = G;
                Controller.Instance.Go();
            });
        }

        ~ObjectConnection()
        {
            Connection.DisposeAsync();
        }

        /// <summary>
        /// Установка объема поступления воздуха на объекте
        /// </summary>
        /// <param name="g"></param>
        public void SendG(double g)
        {
            Connection.InvokeAsync("SetG", g);
        }
    }
}
