using MFS.Controllers.Attributes;
using MFS.Hubs;
using MFS.Models;
using MFS.Repositorys;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Senparc.Weixin.Work.AdvancedAPIs;
using Senparc.Weixin.Work.Containers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Controllers
{
    [Route("api/[controller]"), Axios]
    public class AssayController : ControllerBase
    {
        private readonly AssayRepository r;
        private readonly PurchaseRepository pu_r;
        private readonly IHubContext<PrintHub> _hub;
        private readonly IHostingEnvironment _hostingEnvironment;
        WorkOption option;
        public AssayController(AssayRepository repository, PurchaseRepository pu_repository, IHubContext<PrintHub> hub, IOptionsSnapshot<WorkOption> option, IHostingEnvironment env)
        {
            r = repository;
            pu_r = pu_repository;
            _hub = hub;
            _hostingEnvironment = env;
            this.option = option.Value;
        }
        #region 推送打印指令到指定打印机端
        [NonAction]
        public async Task SendPrintAsync(string who, Assay assay, string actionName)
        {
            foreach (var connectionId in PrintHub.connections.GetConnections(who))
            {
                await _hub.Clients.Client(connectionId).SendAsync(actionName, assay);
            }
        }
        #endregion
        #region GET
        [HttpGet("[action]")]
        public ResultJSON<string> AssayNo()
        {
            return new ResultJSON<string>
            {
                Code = 0,
                Data = r.GetSerialNumber(r.GetLastAssayNo())
            };
        }
        [HttpGet]
        public ResultJSON<List<Assay>> Get()
        {
            return new ResultJSON<List<Assay>>
            {
                Code = 0,
                Data = r.GetAllList().OrderByDescending(a => a.Id).ToList()
            };
        }
        [HttpGet("[action]")]
        public ResultJSON<List<Assay>> GetByPager(int page, int pageSize)
        {
            return new ResultJSON<List<Assay>>
            {
                Code = 0,
                Data = r.LoadPageList(page, pageSize, out int rowCount,true).Include(a => a.Store).Include(a => a.Purchase).ToList()
            };
        }
        [HttpGet("[action]/{sId}")]
        public ResultJSON<List<Assay>> GetByStoreId(int sId)
        {
            return new ResultJSON<List<Assay>>
            {
                Code = 0,
                Data = r.GetAllList(a => a.StoreId == sId).OrderByDescending(a => a.Id).ToList()
            };
        }
        [HttpGet("{sv}")]
        public ResultJSON<List<Assay>> Get(string sv)
        {
            return new ResultJSON<List<Assay>>
            {
                Code = 0,
                Data = r.GetWithInclude(sv).OrderByDescending(a => a.Id).ToList()
            };
        }
        /// <summary>
        /// 向指定打印机推送陆上【化验单】打印指令
        /// </summary>
        /// <param name="id">Order id</param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ResultJSON<Assay>> PrintAssay(int id, string to)
        {
            Assay a = r.GetWithInclude(id);
            await SendPrintAsync(to, a, "printassay");
            return new ResultJSON<Assay>
            {
                Code = 0,
                Data = a
            };
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ResultJSON<string>> ExportExcel(DateTime start, DateTime end)
        {
            try
            {
                List<Assay> list = r.GetAssaysForExportExcel(start, end);
                if (list == null || list.Count == 0)
                    return new ResultJSON<string> { Code = 503, Msg = "没有相关数据" };

                var excellist = new List<AssayExcel>();
                AssayExcel oe;
                #region 赋值到excel model
                foreach (var item in list)
                {
                    oe = new AssayExcel
                    {
                        单号 = item.Name,
                        类型 = Enum.GetName(typeof(AssayType), item.AssayType),
                        油仓 = item.Store == null? "" : item.Store.Name,
                        卸油单 = item.Purchase == null ? "" : item.Purchase.Name,
                        视密 = item.视密,
                        标密 = item.标密,
                        闭口闪点 = item.闭口闪点,
                        油温 = item.Temperature,
                        量油温时间 = item.OilTempTime.ToString("yyyy-MM-dd hh:mm"),
                        味道 = Enum.GetName(typeof(SmellType), item.SmellType),
                        混水反应 = item.混水反应,
                        十六烷值 = item.十六烷值,
                        十六烷指数 = item.十六烷指数,
                        初硫 = item.初硫,
                        百分十 = item.Percentage10,
                        百分五十 = item.Percentage50,
                        百分九十 = item.Percentage90,
                        回流 = item.回流,
                        干点 = item.干点,
                        蚀点 = item.蚀点,
                        凝点 = item.凝点,
                        含硫 = item.含硫,
                        化验员 = item.Assayer,
                        化验时间 = item.CreatedAt.ToString("yyyy-MM-dd hh:mm")

                    };
                    excellist.Add(oe);
                }
                #endregion
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, @"excel\");
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_化验记录.xlsx";
                Helper.FileHelper.ExportExcelByEPPlus(excellist, filePath + fileName);
                string filePathURL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, @"excel/" + fileName);

                //推送到“导出数据”
                this.option.导出数据AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.导出数据Secret);
                await MassApi.SendTextCardAsync(option.导出数据AccessToken, option.导出数据AgentId, $"{UserName}导出化验记录到Excel"
                         , $"<div class=\"gray\">操作时间：{DateTime.Now.ToString()}</div>"
                          + $"<div class=\"gray\">导出时间段：{start.ToString()} - {end.ToString()}</div>"
                         , filePathURL, toUser: "@all");

                return new ResultJSON<string> { Code = 0, Data = filePathURL };
            }
            catch (Exception e)
            {
                return new ResultJSON<string> { Code = 503, Msg = e.Message };
            }
        }
        #endregion
        #region POST
        [HttpPost]
        public ResultJSON<Assay> Post([FromBody]Assay a)
        {
            //判断是否重复单号
            if (r.Has(ass => ass.Name == a.Name))
                return new ResultJSON<Assay> { Code = 502 };

            r.CurrentUser = UserName;
            a.Assayer = UserName;
            var result = r.Insert(a);

            if (a.PurchaseId.HasValue)
            {
                var purchase = pu_r.Get(int.Parse(a.PurchaseId.ToString()));
                purchase.AssayId = result.Id;
                pu_r.Update(purchase);
            }

            return new ResultJSON<Assay>
            {
                Code = 0,
                Data = result
            };
        }
        #endregion
    }
}
