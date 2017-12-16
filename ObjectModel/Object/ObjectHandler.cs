using ObjectModel.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace ObjectModel.Object
{
    public class ObjectHandler
    {
        private static ObjectHandler _instance;

        public static ObjectHandler GetInstance()
        {
            return _instance;
        }

        public static ObjectHandler InitInstance(ObjectHub hub)
        {
            if (_instance == null)
            {
                _instance = new ObjectHandler(hub);
            }
            return _instance;
        }

        private ObjectHub Hub { get; }

        public ObjectValueCalculator ObjectValueCalculator { get; set; }

        private ObjectHandler(ObjectHub hub)
        {
            Hub = hub;
            ObjectValueCalculator = new ObjectValueCalculator(this);
        }

        public void Run()
        {
            ObjectValueCalculator.Run();
        }

        public void SendValue()
        {
            Hub.Meter(ObjectValueCalculator.T, ObjectValueCalculator.G);
        }
    }
}
