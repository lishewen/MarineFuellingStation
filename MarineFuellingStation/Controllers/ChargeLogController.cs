using MFS.Controllers.Attributes;
using MFS.Hubs;
using MFS.Models;
using MFS.Repositorys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Controllers
{
    [Route("api/[controller]"), Axios]
    public class ChargeLogController : ControllerBase
    {
        private readonly ChargeLogRepository r;
        private readonly IHubContext<PrintHub> _hub;
        public ChargeLogController(ChargeLogRepository repository, IHubContext<PrintHub> hub)
        {
            r = repository;
            _hub = hub;
        }
        #region 推送打印指令到指定打印机端
        [NonAction]
        public async Task SendPrintAsync(string who, ChargeLog cl, string actionName)
        {
            foreach (var connectionId in PrintHub.connections.GetConnections(who))
            {
                await _hub.Clients.Client(connectionId).SendAsync(actionName, cl);
            }
        }
        #endregion
        #region GET
        [HttpGet]
        public ResultJSON<List<ChargeLog>> Get()
        {
            return new ResultJSON<List<ChargeLog>>
            {
                Code = 0,
                Data = r.GetAllList()
            };
        }
        /// <summary>
        /// 分页显示数据
        /// </summary>
        /// <param name="page">第N页</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="sv">搜索关键字</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<List<ChargeLog>> GetByPager(int page, int pageSize, string sv = "")
        {
            List<ChargeLog> cl;
            if (string.IsNullOrEmpty(sv))
                cl = r.LoadPageList(page, pageSize, out int rCount, true).Include("Client").Include("Company").OrderByDescending(c => c.Id).ToList();
            else
                cl = r.LoadPageList(page, pageSize, out int rCount, true, false, c => c.Client.CarNo.Contains(sv) || c.Company.Name.Contains(sv)).Include(clog => clog.Client).Include("Company").OrderByDescending(c => c.Id).ToList();
            return new ResultJSON<List<ChargeLog>>
            {
                Code = 0,
                Data = cl
            };
        }
        [HttpGet("{id}")]
        public ResultJSON<ChargeLog> Get(int id)
        {
            ChargeLog s = r.Get(id);
            return new ResultJSON<ChargeLog>
            {
                Code = 0,
                Data = s
            };
        }
        #endregion
        #region PUT
        [HttpPut]
        public ResultJSON<ChargeLog> Put([FromBody]ChargeLog model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<ChargeLog>
            {
                Code = 0,
                Data = r.InsertOrUpdate(model)
            };
        }
        #endregion
        #region POST
        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isCompanyCharge">公司充值或客户充值</param>
        /// <returns></returns>
        [HttpPost]
        public ResultJSON<ChargeLog> Post([FromBody]ChargeLog model)
        {
            r.CurrentUser = UserName;
            ChargeLog c = r.InsertAndUpdateBalances(model);
            if (c == null)
                return new ResultJSON<ChargeLog>
                {
                    Code = 501,
                    Msg = "无法更新客户余额"
                };
            else
                return new ResultJSON<ChargeLog>
                {
                    Code = 0,
                    Data = c
                };
        }
        /// <summary>
        /// 向指定打印机推送【个人预付款确认单】打印指令
        /// </summary>
        /// <param name="cl">ChargeLog model</param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ResultJSON<ChargeLog>> PrintClientPrepay([FromBody]ChargeLog cl, string to)
        {
            await SendPrintAsync(to, cl, "printclientprepayment");
            return new ResultJSON<ChargeLog>
            {
                Code = 0,
                Data = cl
            };
        }
        /// <summary>
        /// 向指定打印机推送【公司预付款确认单】打印指令
        /// </summary>
        /// <param name="cl">ChargeLog model</param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ResultJSON<ChargeLog>> PrintCompanyPrepay([FromBody]ChargeLog cl, string to)
        {
            await SendPrintAsync(to, cl, "printcompanyprepayment");
            return new ResultJSON<ChargeLog>
            {
                Code = 0,
                Data = cl
            };
        }
        #endregion
    }
}
