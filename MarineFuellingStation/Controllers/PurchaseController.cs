using MFS.Controllers.Attributes;
using MFS.Helper;
using MFS.Hubs;
using MFS.Models;
using MFS.Repositorys;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Senparc.Weixin.Work.AdvancedAPIs;
using Senparc.Weixin.Work.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MFS.Controllers
{
    [Route("api/[controller]"), Axios]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseRepository r;
        private readonly IHostingEnvironment _hostingEnvironment;
        WorkOption option;
        private readonly IHubContext<PrintHub> _hub;
        public PurchaseController(PurchaseRepository repository, IOptionsSnapshot<WorkOption> option, IHostingEnvironment env, IHubContext<PrintHub> hub)
        {
            r = repository;
            _hostingEnvironment = env;
            //获取 销售单 企业微信应用的AccessToken
            this.option = option.Value;
            this.option.进油计划AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.进油计划Secret);
            this.option.进油看板AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.进油看板Secret);
            this.option.陆上卸油AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.陆上卸油Secret);

            _hub = hub;
        }
        [HttpGet]
        public ResultJSON<List<Purchase>> Get()
        {
            return new ResultJSON<List<Purchase>>
            {
                Code = 0,
                Data = r.GetIncludeProduct().OrderByDescending(p => p.Id).ToList()
            };
        }
        [HttpGet("[action]/{id}")]
        public ResultJSON<Purchase> GetDetail(int id)
        {
            return new ResultJSON<Purchase>
            {
                Code = 0,
                Data = r.GetDetail(id)
            };
        }
        [HttpGet("[action]/{n}")]
        public ResultJSON<List<Purchase>> GetTopN(int n)
        {
            return new ResultJSON<List<Purchase>>
            {
                Code = 0,
                Data = r.GetTopNPurchases(n)
            };
        }
        [HttpGet("{sv}")]
        public ResultJSON<List<Purchase>> Get(string sv)
        {
            return new ResultJSON<List<Purchase>>
            {
                Code = 0,
                Data = r.GetIncludeProduct().Where(s => s.CarNo.Contains(sv)).ToList()
            };
        }
        [HttpGet("[action]")]
        public ResultJSON<string> PurchaseNo()
        {
            return new ResultJSON<string>
            {
                Code = 0,
                Data = r.GetSerialNumber(r.GetLastPurchaseNo())
            };
        }
        /// <summary>
        /// 获得最近生成的进油单
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<Purchase> LastPurchase()
        {
            return new ResultJSON<Purchase>
            {
                Code = 0,
                Data = r.GetAllList().OrderByDescending(p => p.Id).FirstOrDefault()
            };
        }
        /// <summary>
        /// 准备卸油操作的单据
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<List<Purchase>> GetReadyUnload()
        {

            return new ResultJSON<List<Purchase>>
            {
                Code = 0,

                Data = r.GetReadyUnload()
            };
        }
        /// <summary>
        /// 根据状态显示所有数据
        /// </summary>
        /// <param name="pus">State状态</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<List<Purchase>> GetAllByState(Purchase.UnloadState pus)
        {

            return new ResultJSON<List<Purchase>>
            {
                Code = 0,
                Data = r.GetIncludeProduct().Where(p => p.State == pus).OrderByDescending(p => p.Id).ToList()
            };
        }
        /// <summary>
        /// 根据状态分页显示数据
        /// </summary>
        /// <param name="page">第N页</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pus">State状态</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<List<Purchase>> GetByState(int page, int pageSize, Purchase.UnloadState pus)
        {
            
            return new ResultJSON<List<Purchase>>
            {
                Code = 0,
                Data = r.GetByState(page, pageSize, pus)
            };
        }
        [HttpPut("[action]")]
        public ResultJSON<Purchase> ChangeState([FromBody]Purchase p)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Purchase>
            {
                Code = 0,
                Data = r.Update(p)
            };
        }
        
        [HttpPut("[action]")]
        public ResultJSON<Purchase> UnloadRestart(int pid)
        {
            r.CurrentUser = UserName;
            var purchase = r.Get(pid);
            purchase.State = Purchase.UnloadState.已开单;
            r.Save();
            return new ResultJSON<Purchase>
            {
                Code = 0,
                Data = purchase
            };
        }
        [HttpPost("[action]")]
        public async Task<ResultJSON<string>> UploadFile([FromForm]IFormFile file)
        {
            if (file != null)
            {
                var extName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
                int i = extName.LastIndexOf('.');
                extName = extName.Substring(i);
                string fileName = Guid.NewGuid() + extName;
                var filePath = _hostingEnvironment.WebRootPath + @"\upload\" + fileName;
                await file.SaveAsAsync(filePath);
                return new ResultJSON<string>
                {
                    Code = 0,
                    Data = $"/upload/{fileName}"
                };
            }
            else
            {
                return new ResultJSON<string>
                {
                    Code = 1
                };
            }
        }
        [HttpPost]
        public ResultJSON<Purchase> Post([FromBody]Purchase p)
        {
            r.CurrentUser = UserName;
            var result = r.Insert(p);

            //推送到“进油计划”
            MassApi.SendTextCard(option.进油计划AccessToken, option.进油计划AgentId, "已开进油计划单"
                     , $"<div class=\"gray\">单号：{result.Name}</div>" +
                     $"<div class=\"normal\">运输车号：{result.CarNo}{result.TrailerNo}</div>" +
                     $"<div class=\"normal\">预计到达：{result.ArrivalTime}</div>"
                     , $"http://vue.car0774.com/#/produce/buyboard", toUser: "@all");
            //推送到“进油看板”
            MassApi.SendTextCard(option.进油看板AccessToken, option.进油看板AgentId, "已开进油计划单"
                     , $"<div class=\"gray\">单号：{result.Name}</div>" +
                     $"<div class=\"normal\">运输车号：{result.CarNo}{result.TrailerNo}</div>" +
                     $"<div class=\"normal\">预计到达：{result.ArrivalTime}</div>"
                     , $"https://vue.car0774.com/#/produce/buyboard", toUser: "@all");
            //推送到“陆上卸油”
            MassApi.SendTextCard(option.陆上卸油AccessToken, option.陆上卸油AgentId, "已开进油计划单"
                     , $"<div class=\"gray\">单号：{result.Name}</div>" +
                     $"<div class=\"normal\">运输车号：{result.CarNo}{result.TrailerNo}</div>" +
                     $"<div class=\"normal\">预计到达：{result.ArrivalTime}</div>"
                     , $"https://vue.car0774.com/#/produce/buyboard", toUser: "@all");
            _hub.Clients.All.InvokeAsync("printunload", p);

            return new ResultJSON<Purchase>
            {
                Code = 0,
                Data = result
            };
        }
        /// <summary>
        /// 审核卸油 设置状态State为已审核
        /// </summary>
        /// <param name="sp">model</param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public ResultJSON<Purchase> AuditingOK([FromBody]Purchase pu)
        {
            pu.State = Purchase.UnloadState.已审核;
            //更新油仓相关数量，平均单价，出入仓记录
            if (r.UpdateStoreOil(pu))
            {
                return new ResultJSON<Purchase>
                {
                    Code = 0,
                    Data = r.Update(pu)
                };
            }
            else
                return new ResultJSON<Purchase>
                {
                    Code = 500,
                    Msg = "操作失败"
                };
        }

    }
}
