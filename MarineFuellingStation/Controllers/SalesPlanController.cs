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
    public class SalesPlanController : ControllerBase
    {
        private readonly SalesPlanRepository r;
        private readonly IHubContext _hub;
        public SalesPlanController(SalesPlanRepository repository, IConnectionManager signalRConnectionManager)
        {
            r = repository;
            _hub = signalRConnectionManager.GetHubContext<PrintHub>();
        }

        [HttpGet("[action]")]
        public ResultJSON<string> SalesPlanNo()
        {
            return new ResultJSON<string>
            {
                Code = 0,
                Data = r.GetSerialNumber(r.GetLastSalesPlanNo())
            };
        }
        [HttpPost]
        public ResultJSON<SalesPlan> Post([FromBody]SalesPlan s)
        {
            r.CurrentUser = UserName;
            var result = r.Insert(s);

            //推送打印指令
            _hub.Clients.All.printsalesplan(result);

            return new ResultJSON<SalesPlan>
            {
                Code = 0,
                Data = result
            };
        }
        [HttpGet]
        public ResultJSON<List<SalesPlan>> Get()
        {
            return new ResultJSON<List<SalesPlan>>
            {
                Code = 0,
                Data = r.GetAllList()
            };
        }
    }
}
