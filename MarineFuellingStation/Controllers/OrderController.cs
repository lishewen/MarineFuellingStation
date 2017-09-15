using MFS.Controllers.Attributes;
using MFS.Hubs;
using MFS.Models;
using MFS.Repositorys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IHubContext<PrintHub> _hub;
        public OrderController(OrderRepository repository, IHubContext<PrintHub> hub)
        {
            r = repository;
            _hub = hub;
        }
        [HttpGet("[action]")]
        public async Task<ResultJSON<string>> OrderNo()
        {
            await _hub.Clients.All.InvokeAsync("login", UserName);

            return new ResultJSON<string>
            {
                Code = 0,
                Data = r.GetSerialNumber(r.GetLastOrderNo())
            };
        }
        [HttpPost]
        public async Task<ResultJSON<Order>> Post([FromBody]Order o)
        {
            r.CurrentUser = UserName;
            var result = r.Insert(o);

            //推送打印指令
            await _hub.Clients.All.InvokeAsync("printorder", result);

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
        [HttpGet("{id}")]
        public ResultJSON<Order> Get(int id)
        {
            return new ResultJSON<Order>
            {
                Code = 0,
                Data = r.Get(id)
            };
        }
    }
}
