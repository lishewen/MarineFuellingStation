using MFS.Controllers.Attributes;
using MFS.Hubs;
using MFS.Models;
using MFS.Repositorys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Controllers
{
    [Route("api/[controller]"), Axios]
    public class OrderController : ControllerBase
    {
        private readonly OrderRepository r;
        private readonly IHubContext _hub;
        public OrderController(OrderRepository repository, IConnectionManager signalRConnectionManager)
        {
            r = repository;
            _hub = signalRConnectionManager.GetHubContext<PrintHub>();
        }
        [HttpGet("[action]")]
        public ResultJSON<string> OrderNo()
        {
            return new ResultJSON<string>
            {
                Code = 0,
                Data = r.GetSerialNumber(r.GetLastOrderNo())
            };
        }
        [HttpPost]
        public ResultJSON<Order> Post([FromBody]Order o)
        {
            r.CurrentUser = UserName;
            var result = r.Insert(o);

            //推送打印指令
            _hub.Clients.All.printorder(result);

            return new ResultJSON<Order>
            {
                Code = 0,
                Data = result
            };
        }
        [HttpGet]
        public ResultJSON<List<Order>> Get()
        {
            return new ResultJSON<List<Order>>
            {
                Code = 0,
                Data = r.GetAllList()
            };
        }
    }
}
