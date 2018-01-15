using Microsoft.AspNetCore.SignalR;
using ObjectModel.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObjectModel.Hubs
{
    /// <summary>
    /// Хаб объекта
    /// </summary>
    public class ObjectHub : Hub
    {
        private static ObjectHandler Handler { get; set; }

        /// <summary>
        /// Инициализация и запуск симуляции объекта
        /// </summary>
        public void Init()
        {
            if (Handler == null)
            {
                Handler = ObjectHandler.InitInstance(this);
            }
            Handler.Run();
        }

        /// <summary>
        /// Установка входного значения
        /// </summary>
        /// <param name="value"></param>
        public void SetG(double value)
        {
            Console.WriteLine($"Given G={value}");
            if (Handler == null)
            {
                Handler = ObjectHandler.InitInstance(this);
            }
            Handler.ObjectValueCalculator.G = value;
        }

        /// <summary>
        /// Рассылка значений объекта подключенным приложениям
        /// </summary>
        /// <param name="T"></param>
        /// <param name="G"></param>
        public void Meter(double T, double G)
        {
            this.Clients.All.InvokeAsync("Meter", T, G);
        }
    }
}
