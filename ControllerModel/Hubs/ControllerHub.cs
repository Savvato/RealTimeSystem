using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControllerModel.Hubs
{
    public class ControllerHub : Hub
    {
        public void Start()
        {
            Console.WriteLine("START");

            Controller.Controller.Instance.ConnectionHandler.Connection.SendAsync("Init", new object[0]);
            Controller.Controller.Instance.ConnectionHandler = new Controller.ObjectConnection();
            Controller.Controller.Instance.Go();
        }

        /// <summary>
        /// Установка целевого значения температуры газа
        /// </summary>
        /// <param name="TargetT"></param>
        public void SetTargetT(double TargetT)
        {
            Console.WriteLine("TargetT");
            Controller.Controller.Instance.TargetT = TargetT;
            Controller.Controller.Instance.Go();
        }

        /// <summary>
        /// Измерение параметров объекта
        /// </summary>
        /// <param name="T">Текущая температура газа</param>
        /// <param name="G">Текущий объём поступаемого воздуха</param>
        public void Meter(double T, double G)
        {
            Console.WriteLine($"Meter: T={T}; G={G}");
            Controller.Controller.Instance.CurrentT = T;
            Controller.Controller.Instance.CurrentG = G;
            Controller.Controller.Instance.Go();
        }

        /// <summary>
        /// Запрос данных с клиентского приложения
        /// </summary>
        public void GetData()
        {
            SendDataToClient();
        }

        /// <summary>
        /// Отправка данных на клиент и объект
        /// </summary>
        /// <param name="ControllingError">Ошибка регулирования</param>
        /// <param name="TargetT">Целевое значение температуры газа</param>
        /// <param name="CurrentT">Текущее значение температуры газа</param>
        /// <param name="CurrentG">Текущее значение объёма поступающего воздуха</param>
        public void SendDataToClient()
        {
            this.Clients.All.InvokeAsync(
                "Data", 
                Controller.Controller.Instance.CurrentControllingError,
                Controller.Controller.Instance.TargetT,
                Controller.Controller.Instance.CurrentT,
                Controller.Controller.Instance.CurrentG
            );
        }
    }
}
