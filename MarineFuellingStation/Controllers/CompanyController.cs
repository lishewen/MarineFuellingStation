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
    public class CompanyController : ControllerBase
    {
        private readonly CompanyRepository r;
        public CompanyController(CompanyRepository repository)
        {
            r = repository;
        }
        [HttpPost]
        public ResultJSON<Company> Post([FromBody]Company model)
        {
            if (r.Has(c => c.Name == model.Name)) return new ResultJSON<Company> { Code = 0, Msg = "操作失败，已存在" + model.Name };
            r.CurrentUser = UserName;
            return new ResultJSON<Company>
            {
                Code = 0,
                Data = r.Insert(model)
            };
        }
        [HttpGet]
        public ResultJSON<List<Company>> Get()
        {
            return new ResultJSON<List<Company>>
            {
                Code = 0,
                Data = r.GetAllList()
            };
        }
        [HttpGet("{sv}")]
        public ResultJSON<List<Company>> Get(string sv)
        {
            return new ResultJSON<List<Company>>
            {
                Code = 0,
                Data = r.GetAllList(s => s.Name.Contains(sv))
            };
        }
        /// <summary>
        /// 只根据company表内字段搜索关键字
        /// </summary>
        /// <param name="kw">电话|名称关键字</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<List<Company>> GetByCompanyKeyword(string kw)
        {
            List<Company> ls = r.GetAllList(c => 
                c.Name.Contains(kw)
                || c.Phone == kw);
            return new ResultJSON<List<Company>>
            {
                Code = 0,
                Data = ls
            };
        }
    }
}
