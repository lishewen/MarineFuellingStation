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
            r.CurrentUser = UserName;
            model.CreatedBy = UserName;
            model.CreatedAt = DateTime.Now;
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
    }
}
