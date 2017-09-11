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
    public class AssayController : ControllerBase
    {
        private readonly AssayRepository r;
        public AssayController(AssayRepository repository)
        {
            r = repository;
        }
        [HttpGet("[action]")]
        public ResultJSON<string> AssayNo()
        {
            return new ResultJSON<string>
            {
                Code = 0,
                Data = r.GetSerialNumber(r.GetLastAssayNo())
            };
        }
        [HttpPost]
        public ResultJSON<Assay> Post([FromBody]Assay a)
        {
            r.CurrentUser = UserName;
            a.Assayer = UserName;
            var result = r.Insert(a);

            return new ResultJSON<Assay>
            {
                Code = 0,
                Data = result
            };
        }
        [HttpGet]
        public ResultJSON<List<Assay>> Get()
        {
            return new ResultJSON<List<Assay>>
            {
                Code = 0,
                Data = r.GetAllList()
            };
        }
        [HttpGet("{sv}")]
        public ResultJSON<List<Assay>> Get(string sv)
        {
            return new ResultJSON<List<Assay>>
            {
                Code = 0,
                Data = r.GetAllList(s => s.Name.Contains(sv))
            };
        }
    }
}
