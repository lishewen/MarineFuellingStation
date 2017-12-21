using MFS.Controllers.Attributes;
using MFS.Models;
using MFS.Repositorys;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Controllers
{
    [Route("api/[controller]"), Axios]
    public class NoticeController : ControllerBase
    {
        private readonly NoticeRepository r;
        public NoticeController(NoticeRepository repository)
        {
            r = repository;
        }
        [HttpPost]
        public ResultJSON<Notice> Post([FromBody]Notice model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Notice>
            {
                Code = 0,
                Data = r.Insert(model)
            };
        }
        #region GET
        [HttpGet]
        public ResultJSON<List<Notice>> Get()
        {
            return new ResultJSON<List<Notice>>
            {
                Code = 0,
                Data = r.GetAllList()
            };
        }
        [HttpGet("[action]")]
        public ResultJSON<List<Notice>> GetByIsUse(int page, int pageSize, bool isUse)
        {
            return new ResultJSON<List<Notice>>
            {
                Code = 0,
                Data = r.GetAllList().Where(n => n.IsUse == isUse).OrderByDescending(n => n.LastUpdatedAt).ToList()
            };
        }
        [HttpGet("{sv}")]
        public ResultJSON<List<Notice>> Get(string sv)
        {
            return new ResultJSON<List<Notice>>
            {
                Code = 0,
                Data = r.GetAllList(s => s.Name.Contains(sv))
            };
        }
        /// <summary>
        /// 根据应用名称获得数据
        /// </summary>
        /// <param name="app">应用名称</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<Notice> GetByAppName(string app)
        {
            return new ResultJSON<Notice>
            {
                Code = 0,
                Data = r.GetAllList(n => n.ToApps.Contains(app) && n.IsUse == true).OrderByDescending(n => n.LastUpdatedAt).FirstOrDefault()
            };
        }
        #endregion
        #region PUT
        /// <summary>
        /// 变更通知是否启用
        /// </summary>
        /// <param name="model">Notice model</param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public ResultJSON<Notice> ChangeIsUse([FromBody]Notice model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Notice>
            {
                Code = 0,
                Data = r.Update(model)
            };
        }
        #endregion
    }
}
