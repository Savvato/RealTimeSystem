using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControllerModel.Hubs
{
    public class ControllerHub : Hub
    {
        /// <summary>
        /// Запуск систем симуляции
        /// </summary>
        public void Start()
        {
            Console.WriteLine("START");

            Controller.Controller.Instance.ConnectionHandler.Connection.SendAsync("Init", new object[0]);
            Controller.Controller.Instance.ConnectionHandler = new Controller.ObjectConnection();
        }

        /// <summary>
        /// Включение/выключение контроллера
        /// </summary>
        /// <param name="isControllerEnabled"></param>
        public void EnableController(bool isControllerEnabled)
        {
            Controller.Controller.Instance.IsEnabled = isControllerEnabled;
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
        /// Запрос данных с клиентского приложения
        /// </summary>
        public void GetData()
        {
            SendDataToClient();
        }

        /// <summary>
        /// Отправка данных на клиент и объект
        /// </summary>
        public void SendDataToClient()
        {
            this.Clients.All.InvokeAsync(
                "Data", 
                Controller.Controller.Instance.IsEnabled,
                Controller.Controller.Instance.CurrentControllingError,
                Controller.Controller.Instance.TargetT,
                Controller.Controller.Instance.CurrentT,
                Controller.Controller.Instance.CurrentG
            );
        }
    }
}
