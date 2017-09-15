using MFS.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Hubs
{
    public class PrintHub : Hub
    {
        public Task PrintSalesPlan(SalesPlan model)
        {
            return Clients.All.InvokeAsync("printsalesplan", model);
        }
        public Task PrintOrder(Order model)
        {
            return Clients.All.InvokeAsync("printorder", model);
        }
        public Task Login(string username)
        {
            return Clients.All.InvokeAsync("login", username);
        }
    }
}
