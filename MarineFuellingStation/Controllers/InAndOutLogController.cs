using MFS.Controllers.Attributes;
using MFS.Models;
using MFS.Repositorys;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
    public class InAndOutLogController : ControllerBase
    {
        private readonly InAndOutLogRepository r;
        private readonly IHostingEnvironment _hostingEnvironment;
        WorkOption option;
        public InAndOutLogController(InAndOutLogRepository repository, IHostingEnvironment env, IOptionsSnapshot<WorkOption> option)
        {
            r = repository;
            _hostingEnvironment = env;
            this.option = option.Value;
        }
        #region POST
        [HttpPost]
        public ResultJSON<InAndOutLog> Post([FromBody]InAndOutLog model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<InAndOutLog>
            {
                Code = 0,
                Data = r.Insert(model)
            };
        }
        #endregion
        #region GET
        [HttpGet("[action]")]
        public ResultJSON<List<InAndOutLog>> GetIncludeStore(LogType type = LogType.全部 ,int page = 1)
        {

            return new ResultJSON<List<InAndOutLog>>
            {
                Code = 0,
                Data = r.GetIncludeStore(type,page)
            };
        }
        [HttpGet("{sv}")]
        public ResultJSON<List<InAndOutLog>> Get(string sv)
        {
            return new ResultJSON<List<InAndOutLog>>
            {
                Code = 0,
                Data = r.GetAllList(s => s.Name.Contains(sv))
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
                List<InAndOutLog> list = r.GetAllList(m => m.CreatedAt >= start && m.CreatedAt <= end);
                if (list == null || list.Count == 0)
                    return new ResultJSON<string> { Code = 503, Msg = "没有相关数据" };

                var excellist = new List<InAndOutLogExcel>();
                InAndOutLogExcel me;
                #region 赋值到excel model
                foreach (var item in list)
                {
                    me = new InAndOutLogExcel
                    {
                        操作 = item.Name,
                        出仓入仓 = Enum.GetName(typeof(LogType), item.Type),
                        油仓 = item.Store == null? "" : item.Store.Name,
                        数量 = item.Value,
                        单位 = item.Unit,
                        数量升数 = item.ValueLitre,
                        操作人员 = item.Operators,
                        时间 = item.CreatedAt.ToString("yyyy-MM-dd hh:mm")
                    };
                    excellist.Add(me);
                }
                #endregion
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, @"excel\");
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_出入仓记录.xlsx";
                Helper.FileHelper.ExportExcelByEPPlus(excellist, filePath + fileName);
                string filePathURL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, @"excel/" + fileName);

                //推送到“导出数据”
                this.option.导出数据AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.导出数据Secret);
                await MassApi.SendTextCardAsync(option.导出数据AccessToken, option.导出数据AgentId, $"{UserName}导出出入仓记录数据到Excel"
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
    }
}
