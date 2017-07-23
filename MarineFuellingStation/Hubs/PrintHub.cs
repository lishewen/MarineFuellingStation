using MFS.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Hubs
{
    [HubName("print")]
    public class PrintHub : Hub
    {
        public void PrintSalesPlan(SalesPlan model)
        {
            Clients.All.printsalesplan(model);
        }
    }
}
