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
    public class SalesPlanController : ControllerBase
    {
        private readonly SalesPlanRepository r;
        private readonly ClientRepository cr;
        private readonly IHubContext<PrintHub> _hub;
        WorkOption option;
        public SalesPlanController(SalesPlanRepository repository, IHubContext<PrintHub> hub, IOptionsSnapshot<WorkOption> option, ClientRepository clientRepository)
        {
            r = repository;
            cr = clientRepository;
            _hub = hub;
            //获取 销售计划 企业微信应用的AccessToken
            this.option = option.Value;
            this.option.销售计划AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.销售计划Secret);
            this.option.审核AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.审核Secret);
        }

        [HttpGet("SalesPlanNo")]
        public ResultJSON<string> SalesPlanNo()
        {
            return new ResultJSON<string>
            {
                Code = 0,
                Data = r.GetSerialNumber(r.GetLastSalesPlanNo())
            };
        }
        [HttpPost]
        public async Task<ResultJSON<SalesPlan>> Post([FromBody]SalesPlan s)
        {
            r.CurrentUser = UserName;

            //当车号/船号没有对应的客户资料时，自动新增客户资料，以便我的客户中的关联查找
            if (!cr.AddClientWithNoFind(s.CarNo, UserName, s.ProductId))
                return new ResultJSON<SalesPlan> { Code = 501, Msg = "无法新增该客户，请联系开发人员" };
            
            SalesPlan result = r.Insert(s);

            //推送打印指令
            await _hub.Clients.All.InvokeAsync("printsalesplan", result);
            //推送企业微信卡片消息（最多5行，128个字符）
            MassApi.SendTextCard(option.销售计划AccessToken, option.销售计划AgentId, "制定销售计划成功"
                     , $"<div class=\"gray\">单号：{result.Name}</div>" +
                     $"<div class=\"normal\">开单人：{UserName}</div>" +
                     $"<div class=\"normal\">船号/车号：{result.CarNo}</div>" +
                     $"<div class=\"normal\">油品：{result.OilName}</div>"
                     , $"http://vue.car0774.com/#/sales/plan/{result.Id}/plan", toUser: "@all");

            //推送到“审核”
            MassApi.SendTextCard(option.审核AccessToken, option.审核AgentId, "已开单，待审核"
                     , $"<div class=\"gray\">单号：{result.Name}</div>" +
                     $"<div class=\"normal\">开单人：{UserName}</div>" +
                     $"<div class=\"normal\">船号/车号：{result.CarNo}</div>" +
                     $"<div class=\"normal\">油品：{result.OilName}</div>"
                     , $"http://vue.car0774.com/#/sales/auditing", toUser: "@all");

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
        /// <summary>
        /// 分页显示数据
        /// </summary>
        /// <param name="page">第N页</param>
        /// <param name="pageSize">页记录数</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<List<SalesPlan>> GetByPager(int page, int pageSize)
        {
            return new ResultJSON<List<SalesPlan>>
            {
                Code = 0,
                Data = r.LoadPageList(page, pageSize, out int rCount, true).OrderByDescending(s => s.Id).ToList()
            };
        }
        /// <summary>
        /// 根据状态分页显示数据
        /// </summary>
        /// <param name="page">第N页</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="sps">State状态</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<List<SalesPlan>> GetByState(int page, int pageSize, SalesPlanState sps)
        {
            return new ResultJSON<List<SalesPlan>>
            {
                Code = 0,
                Data = r.LoadPageList(page, pageSize, out int rCount, true, s => s.State == sps).OrderByDescending(s => s.Id).ToList()
            };
        }
        [HttpGet("[action]/{id}")]
        public ResultJSON<SalesPlan> GetDetail(int id)
        {
            return new ResultJSON<SalesPlan>
            {
                Code = 0,
                Data = r.GetDetail(id)
            };
        }
        [HttpGet("[action]")]
        public ResultJSON<List<SalesPlan>> GetHasFinish()
        {
            return new ResultJSON<List<SalesPlan>>
            {
                Code = 0,
                Data = r.GetAllList(s => s.State == SalesPlanState.已审批 || s.State == SalesPlanState.未审批)
            };
        }
        [HttpGet("{sv}")]
        public ResultJSON<List<SalesPlan>> Get(string sv)
        {
            return new ResultJSON<List<SalesPlan>>
            {
                Code = 0,
                Data = r.GetAllList(s => s.CarNo.Contains(sv))
            };
        }
        /// <summary>
        /// 审核计划 设置状态State为已审核
        /// </summary>
        /// <param name="sp">model</param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public ResultJSON<SalesPlan> AuditingOK([FromBody]SalesPlan sp)
        {
            sp.State = SalesPlanState.已审批;
            return new ResultJSON<SalesPlan>
            {
                Code = 0,
                Data = r.Update(sp)
            };
        }
    }
}
