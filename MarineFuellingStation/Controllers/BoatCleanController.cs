using MFS.Controllers.Attributes;
using MFS.Hubs;
using MFS.Models;
using MFS.Repositorys;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
    public class BoatCleanController:ControllerBase
    {
        private readonly BoatCleanRepository r;
        private readonly IHubContext<PrintHub> _hub;
        private readonly IHostingEnvironment _hostingEnvironment;
        WorkOption option;
        public BoatCleanController(BoatCleanRepository repository, IOptionsSnapshot<WorkOption> option, IHubContext<PrintHub> hub, IHostingEnvironment env)
        {
            r = repository;
            _hub = hub;
            _hostingEnvironment = env;
            this.option = option.Value;
        }
        #region 推送打印指令到指定打印机端
        [NonAction]
        public async Task SendPrintAsync(string who, BoatClean bc, string actionName)
        {
            foreach (var connectionId in PrintHub.connections.GetConnections(who))
            {
                await _hub.Clients.Client(connectionId).SendAsync(actionName, bc);
            }
        }
        #endregion
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
        /// <summary>
        /// 分页显示数据
        /// </summary>
        /// <param name="page">第N页</param>
        /// <param name="pageSize">页记录数</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<List<BoatClean>> GetByPager(int page, int pageSize)
        {
            return new ResultJSON<List<BoatClean>>
            {
                Code = 0,
                Data = r.LoadPageList(page, pageSize, out int rCount, true).OrderByDescending(s => s.Id).ToList()
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
        /// 指定目标推送完工证打印指令
        /// </summary>
        /// <param name="id">BoatClean id</param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ResultJSON<BoatClean>> PrintBoatClean(int id, string to)
        {
            BoatClean b = r.Get(id);
            await SendPrintAsync(to, b, "printboatclean");
            return new ResultJSON<BoatClean>
            {
                Code = 0,
                Data = b
            };
        }
        /// <summary>
        /// 指定目标推送收款单打印指令
        /// </summary>
        /// <param name="id">BoatClean id</param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ResultJSON<BoatClean>> PrintBcCollection(int id, string to)
        {
            BoatClean b = r.Get(id);
            await SendPrintAsync(to, b, "printboatcleancollection");
            return new ResultJSON<BoatClean>
            {
                Code = 0,
                Data = b
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
                List<BoatClean> list = r.GetAllList(b => b.CreatedAt >= start && b.CreatedAt <= end);
                if (list == null || list.Count == 0)
                    return new ResultJSON<string> { Code = 503, Msg = "没有相关数据" };

                var excellist = new List<BoatCleanExcel>();
                BoatCleanExcel be;
                #region 赋值到excel model
                foreach (var item in list)
                {
                    be = new BoatCleanExcel
                    {
                        单号 = item.Name,
                        船号 = item.CarNo,
                        金额 = item.Money,
                        航次 = item.Voyage,
                        吨位 = item.Tonnage,
                        批文号 = item.ResponseId,
                        作业地点 = item.Address,
                        作业单位 = item.Company,
                        联系电话 = item.Phone,
                        是否开票 = item.IsInvoice ? "开票" : "",
                        开票单位 = item.BillingCompany,
                        开票单价 = item.BillingPrice,
                        开票数量 = item.BillingCount,
                        支付状态 = Enum.GetName(typeof(BoatCleanPayState),item.PayState),
                        支付金额和方式 = "",
                        施工人员 = item.Worker,
                        开单时间 = item.CreatedAt.ToString("yyyy-MM-dd hh:mm")
                    };
                    excellist.Add(be);
                }
                #endregion
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, @"excel\");
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_船舶清污单.xlsx";
                Helper.FileHelper.ExportExcelByEPPlus(excellist, filePath + fileName);
                string filePathURL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, @"excel/" + fileName);

                //推送到“导出数据”
                this.option.导出数据AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.导出数据Secret);
                await MassApi.SendTextCardAsync(option.导出数据AccessToken, option.导出数据AgentId, $"{UserName}导出船舶清污单数据到Excel"
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
        public ResultJSON<BoatClean> Post([FromBody]BoatClean b)
        {
            //判断是否重复单号
            if (r.Has(bo => bo.Name == b.Name))
                return new ResultJSON<BoatClean> { Code = 502 };

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
