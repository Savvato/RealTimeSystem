using Microsoft.AspNetCore.SignalR;
using ObjectModel.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObjectModel.Hubs
{
    public class ObjectHub : Hub
    {
        private static ObjectHandler Handler { get; set; }

        public void Init()
        {
            if (Handler == null)
            {
                Handler = ObjectHandler.InitInstance(this);
            }
            Handler.Run();
        }

        public void SetG(double value)
        {
            Console.WriteLine($"Given G={value}");
            if (Handler == null)
            {
                Handler = ObjectHandler.InitInstance(this);
            }
            Handler.ObjectValueCalculator.G = value;
        }

        public void Meter(double T, double G)
        {
            this.Clients.All.InvokeAsync("Meter", T, G);
        }
    }
}
