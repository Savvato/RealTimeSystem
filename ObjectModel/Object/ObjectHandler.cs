using ObjectModel.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace ObjectModel.Object
{
    /// <summary>
    /// Класс-посредник между хабом и расчетным классом
    /// Реализует обмен данными
    /// </summary>
    public class ObjectHandler
    {
        private static ObjectHandler _instance;

        /// <summary>
        /// Получение сущности класса (синглтон)
        /// </summary>
        /// <returns></returns>
        public static ObjectHandler GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// Инициализация сущности
        /// </summary>
        /// <param name="hub"></param>
        /// <returns></returns>
        public static ObjectHandler InitInstance(ObjectHub hub)
        {
            if (_instance == null)
            {
                _instance = new ObjectHandler(hub);
            }
            return _instance;
        }

        /// <summary>
        /// Ссылка на хаб
        /// </summary>
        private ObjectHub Hub { get; }

        /// <summary>
        /// Ссылка на расчетный класс
        /// </summary>
        public ObjectValueCalculator ObjectValueCalculator { get; set; }

        private ObjectHandler(ObjectHub hub)
        {
            Hub = hub;
            ObjectValueCalculator = new ObjectValueCalculator(this);
        }

        /// <summary>
        /// Запуск расчетов
        /// </summary>
        public void Run()
        {
            ObjectValueCalculator.Run();
        }

        /// <summary>
        /// Отправка значения подключенным приложениям
        /// </summary>
        public void SendValue()
        {
            Hub.Meter(ObjectValueCalculator.T, ObjectValueCalculator.G);
        }
    }
}
