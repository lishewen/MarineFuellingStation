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
            this.option.采购计划AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.采购计划Secret);
            this.option.采购看板AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.采购看板Secret);
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
        /// 获得最近生成的采购单
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
        [HttpPut("[action]")]
        public ResultJSON<Purchase> ChangeState([FromBody]Purchase p)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Purchase>
            {
                Code = 0,
                Data = r.ChangeState(p)
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

            //推送到“采购计划”
            MassApi.SendTextCard(option.采购计划AccessToken, option.采购计划AgentId, "已开采购计划单"
                     , $"<div class=\"gray\">单号：{result.Name}</div>" +
                     $"<div class=\"normal\">运输车号：{result.CarNo}{result.TrailerNo}</div>" +
                     $"<div class=\"normal\">预计到达：{result.ArrivalTime}</div>"
                     , $"https://vue.car0774.com/#/produce/buyboard", toUser: "@all");
            //推送到“采购看板”
            MassApi.SendTextCard(option.采购看板AccessToken, option.采购看板AgentId, "已开采购计划单"
                     , $"<div class=\"gray\">单号：{result.Name}</div>" +
                     $"<div class=\"normal\">运输车号：{result.CarNo}{result.TrailerNo}</div>" +
                     $"<div class=\"normal\">预计到达：{result.ArrivalTime}</div>"
                     , $"https://vue.car0774.com/#/produce/buyboard", toUser: "@all");
            //推送到“陆上卸油”
            MassApi.SendTextCard(option.陆上卸油AccessToken, option.陆上卸油AgentId, "已开采购计划单"
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
    }
}
