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
    public class MoveStoreController : ControllerBase
    {
        private readonly MoveStoreRepository r;
        private readonly IHubContext<PrintHub> _hub;
        private readonly IHostingEnvironment _hostingEnvironment;
        WorkOption option;
        public MoveStoreController(MoveStoreRepository repository, IOptionsSnapshot<WorkOption> option, IHubContext<PrintHub> hub, IHostingEnvironment env)
        {
            r = repository;
            _hub = hub;
            _hostingEnvironment = env;
            this.option = option.Value;
        }
        [NonAction]
        public async Task SendPrintMoveStoreAsync(string who, MoveStore ms)
        {
            foreach (var connectionId in PrintHub.connections.GetConnections(who))
            {
                await _hub.Clients.Client(connectionId).SendAsync("printmovestore", ms);
            }
        }
        #region GET
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
        [HttpGet("[action]")]
        public ResultJSON<List<MoveStore>> GetByPager(int page, int pagesize, string sv = "")
        {
            List<MoveStore> list;
            if(string.IsNullOrEmpty(sv))
                list = r.LoadPageList(page, pagesize, out int rowCount, true).ToList();
            else
                list = r.LoadPageList(page, pagesize, out int rowCount, true, false, m => m.Name.Contains(sv)).ToList();
            return new ResultJSON<List<MoveStore>>()
            {
                Code = 0,
                Data = list
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
                List<MoveStore> list = r.GetAllList(m => m.CreatedAt >= start && m.CreatedAt <= end);
                if (list == null || list.Count == 0)
                    return new ResultJSON<string> { Code = 503, Msg = "没有相关数据" };

                var excellist = new List<MoveStoreExcel>();
                MoveStoreExcel me;
                #region 赋值到excel model
                foreach (var item in list)
                {
                    me = new MoveStoreExcel
                    {
                        单号 = item.Name,
                        状态 = Enum.GetName(typeof(MoveStoreState),item.State),
                        生产员 = item.Worker,
                        转出仓 = item.OutStoreName,
                        转出油温 = item.OutTemperature,
                        转出密度 = item.OutDensity,
                        计划转出升数 = item.OutPlan,
                        实际转出升数 = item.OutFact,
                        转入仓 = item.InStoreName,
                        转入油温 = item.InTemperature,
                        转入密度 = item.InDensity,
                        创建时间 = item.CreatedAt.ToString("yyyy-MM-dd hh:mm")
                    };
                    excellist.Add(me);
                }
                #endregion
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, @"excel\");
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_转仓单.xlsx";
                Helper.FileHelper.ExportExcelByEPPlus(excellist, filePath + fileName);
                string filePathURL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, @"excel/" + fileName);

                //推送到“导出数据”
                this.option.导出数据AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.导出数据Secret);
                await MassApi.SendTextCardAsync(option.导出数据AccessToken, option.导出数据AgentId, $"{UserName}导出转仓单数据到Excel"
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
        #region PUT
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
                     , $"https://vue.car0774.com/#/oilstore/inout", toUser: "@all");

            return new ResultJSON<MoveStore>
            {
                Code = 0,
                Data = result
            };
        }
        #endregion
        #region POST
        [HttpPost]
        public ResultJSON<MoveStore> Post([FromBody]MoveStore m)
        {
            //判断是否重复单号
            if (r.Has(ms => ms.Name == m.Name))
                return new ResultJSON<MoveStore> { Code = 502 };

            r.CurrentUser = UserName;
            var result = r.Insert(m);

            return new ResultJSON<MoveStore>
            {
                Code = 0,
                Data = result
            };
        }
        #endregion
    }
}
