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
        private ObjectHandler Handler { get; set; }

        public void Init()
        {
            if (Handler == null)
            {
                Handler = ObjectHandler.InitInstance(this);
                Handler.Run();
            }
        }

        public void SetG(double value)
        {
            if (Handler == null)
            {
                Handler = ObjectHandler.InitInstance(this);
            }
            Handler.ObjectValueCalculator.G = value;
        }

        public async Task Send(double T, double G)
        {
            await this.Clients.All.InvokeAsync("Send", T, G);
        }
    }
}
