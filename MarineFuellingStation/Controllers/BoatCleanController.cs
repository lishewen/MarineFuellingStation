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
    public class BoatCleanController:ControllerBase
    {
        private readonly BoatCleanRepository r;
        private readonly IHubContext<PrintHub> _hub;
        public BoatCleanController(BoatCleanRepository repository, IHubContext<PrintHub> hub)
        {
            r = repository;
            _hub = hub;
        }
        [NonAction]
        public async Task SendPrintBoatCleanAsync(string who, BoatClean bc)
        {
            foreach (var connectionId in PrintHub.connections.GetConnections(who))
            {
                await _hub.Clients.Client(connectionId).InvokeAsync("printboatcleancollection", bc);
            }
        }
        #region GET
        [HttpGet("[action]")]
        public ResultJSON<string> BoatCleanNo()
        {
            return new ResultJSON<string>
            {
                Code = 0,
                Data = r.GetSerialNumber(r.GetLastBoatCleanNo())
            };
        }
        [HttpGet]
        public ResultJSON<List<BoatClean>> Get()
        {
            return new ResultJSON<List<BoatClean>>
            {
                Code = 0,
                Data = r.GetAllList()
            };
        }
        [HttpGet("{sv}")]
        public ResultJSON<List<BoatClean>> Get(string sv)
        {
            return new ResultJSON<List<BoatClean>>
            {
                Code = 0,
                Data = r.GetAllList(s => s.CarNo.Contains(sv))
            };
        }
        /// <summary>
        /// 根据付款状态获取分页数据
        /// </summary>
        /// <param name="payState">付款状态</param>
        /// <param name="page">第n页</param>
        /// <param name="pageSize">分页记录数</param>
        /// <param name="searchVal">搜索关键字</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<List<BoatClean>> GetByPayState(BoatCleanPayState payState, int page, int pageSize, string searchVal = "")
        {
            return new ResultJSON<List<BoatClean>>
            {
                Code = 0,
                Data = r.GetByPayState(payState, page, pageSize, searchVal)//每页30条记录
            };
        }
        /// <summary>
        /// 指定目标推送打印指令
        /// </summary>
        /// <param name="id">Order id</param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ResultJSON<BoatClean>> PrintTo(int id, string to)
        {
            BoatClean o = r.Get(id);
            await SendPrintBoatCleanAsync(to, o);
            return new ResultJSON<BoatClean>
            {
                Code = 0,
                Data = o
            };
        }
        #endregion
        #region POST
        [HttpPost]
        public ResultJSON<BoatClean> Post([FromBody]BoatClean b)
        {
            r.CurrentUser = UserName;
            var result = r.Insert(b);

            return new ResultJSON<BoatClean>
            {
                Code = 0,
                Data = result
            };
        }
        #endregion
        #region PUT
        /// <summary>
        /// 订单结算
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public ResultJSON<BoatClean> Pay([FromBody] BoatClean model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<BoatClean>
            {
                Code = 0,
                Data = r.Pay(model)
            };
        }
        /// <summary>
        /// 挂账
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public ResultJSON<BoatClean> PayOnCredit([FromBody] BoatClean model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<BoatClean>
            {
                Code = 0,
                Data = r.Update(model)
            };
        }
        #endregion
    }
}
