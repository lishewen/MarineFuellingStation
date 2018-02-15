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
    public class SurveyController : ControllerBase
    {
        private readonly SurveyRepository r;
        private readonly IHostingEnvironment _hostingEnvironment;
        WorkOption option;
        public SurveyController(SurveyRepository repository, IOptionsSnapshot<WorkOption> option, IHostingEnvironment env)
        {
            r = repository;
            _hostingEnvironment = env;
            this.option = option.Value;
        }
        #region POST
        [HttpPost]
        public ResultJSON<Survey> Post([FromBody]Survey model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Survey>
            {
                Code = 0,
                Data = r.Insert(model)
            };
        }
        #endregion
        #region GET
        [HttpGet]
        public ResultJSON<List<Survey>> Get()
        {
            return new ResultJSON<List<Survey>>
            {
                Code = 0,
                Data = r.GetAllList()
            };
        }
        [HttpGet("{sv}")]
        public ResultJSON<List<Survey>> Get(string sv)
        {
            return new ResultJSON<List<Survey>>
            {
                Code = 0,
                Data = r.GetAllList(s => s.Name.Contains(sv))
            };
        }
        [HttpGet("[action]/{stid}")]
        public ResultJSON<List<Survey>> GetTop15(int stid)
        {
            return new ResultJSON<List<Survey>>
            {
                Code = 0,
                Data = r.Top15(stid)
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
                List<Survey> list = r.GetAllList(s => s.CreatedAt >=start && s.CreatedAt <= end);
                if (list == null || list.Count == 0)
                    return new ResultJSON<string> { Code = 503, Msg = "没有相关数据" };

                var excellist = new List<SurveyExcel>();
                SurveyExcel oe;
                #region 赋值到excel model
                foreach (var item in list)
                {
                    oe = new SurveyExcel
                    {
                        油仓名称 = item.Store == null? "" : item.Store.Name,
                        油温 = item.Temperature,
                        密度 = item.Density,
                        油高 = item.Temperature,
                        油高对应升数 = item.Temperature,
                        测量时间 = item.CreatedAt.ToString("yyyy-MM-dd hh:mm"),
                    };
                    excellist.Add(oe);
                }
                #endregion
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, @"excel\");
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_测量记录.xlsx";
                Helper.FileHelper.ExportExcelByEPPlus(excellist, filePath + fileName);
                string filePathURL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, @"excel/" + fileName);

                //推送到“导出数据”
                this.option.导出数据AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.导出数据Secret);
                await MassApi.SendTextCardAsync(option.导出数据AccessToken, option.导出数据AgentId, $"{UserName}导出油仓测量记录数据到Excel"
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
