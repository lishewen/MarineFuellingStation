using MFS.Controllers.Attributes;
using MFS.Hubs;
using MFS.Models;
using MFS.Repositorys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Senparc.Weixin.Work.AdvancedAPIs;
using Senparc.Weixin.Work.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Controllers
{
    [Route("api/[controller]"), Axios]
    public class MoveStoreController : ControllerBase
    {
        private readonly MoveStoreRepository r;
        private readonly IHubContext<PrintHub> _hub;
        WorkOption option;
        public MoveStoreController(MoveStoreRepository repository, IOptionsSnapshot<WorkOption> option, IHubContext<PrintHub> hub)
        {
            r = repository;
            _hub = hub;
            this.option = option.Value;
        }
        [NonAction]
        public async Task SendPrintMoveStoreAsync(string who, MoveStore ms)
        {
            foreach (var connectionId in PrintHub.connections.GetConnections(who))
            {
                await _hub.Clients.Client(connectionId).InvokeAsync("printmovestore", ms);
            }
        }
        [HttpGet("[action]")]
        public ResultJSON<string> MoveStoreNo()
        {
            return new ResultJSON<string>
            {
                Code = 0,
                Data = r.GetSerialNumber(r.GetLastMoveStoreNo())
            };
        }
        [HttpGet]
        public ResultJSON<List<Models.GET.MoveStore>> Get(bool isFinished)
        {
            return new ResultJSON<List<Models.GET.MoveStore>>()
            {
                Code = 0,
                Data = r.GetForIsFinished(isFinished)
            };
        }
        /// <summary>
        /// 指定目标推送打印指令
        /// </summary>
        /// <param name="id">Order id</param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ResultJSON<MoveStore>> PrintMoveStore(int id, string to)
        {
            MoveStore bc = r.Get(id);
            await SendPrintMoveStoreAsync(to, bc);
            return new ResultJSON<MoveStore>
            {
                Code = 0,
                Data = bc
            };
        }
        /// <summary>
        /// 生产过程切换状态
        /// </summary>
        /// <returns></returns>
        [HttpPut("[action]")]
        public ResultJSON<MoveStore> ChangeState([FromBody]MoveStore m)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<MoveStore>
            {
                Code = 0,
                Data = r.UpdateState(m)
            };
        }

        /// <summary>
        /// 更新实际转入和实际转出
        /// </summary>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<ResultJSON<MoveStore>> UpdateInOutFact([FromBody]MoveStore m)
        {
            r.CurrentUser = UserName;

            var result = r.UpdateInOutFact(m);

            //推送到“油仓情况”
            this.option.油仓情况AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.油仓情况Secret);
            await MassApi.SendTextCardAsync(option.油仓情况AccessToken, option.油仓情况AgentId, "转仓生产完工，已更新油仓油量"
                     , $"<div class=\"gray\">单号：{result.Name}</div>" +
                     $"<div class=\"normal\">施工人：{result.LastUpdatedBy}</div>" +
                     $"<div class=\"normal\">转出：{result.OutStoreName} - {result.OutFact}升</div>" +
                     $"<div class=\"normal\">转入：{result.InStoreName} - {result.InFact}升</div>"
                     , $"https://vue.car0774.com/#/sales/order/{result.Id}/order", toUser: "@all");

            //打印生产转仓单
            await _hub.Clients.All.InvokeAsync("printmovestore", result);

            return new ResultJSON<MoveStore>
            {
                Code = 0,
                Data = result
            };
        }
        [HttpPost]
        public ResultJSON<MoveStore> Post([FromBody]MoveStore m)
        {
            r.CurrentUser = UserName;
            var result = r.Insert(m);

            return new ResultJSON<MoveStore>
            {
                Code = 0,
                Data = result
            };
        }
    }
}
