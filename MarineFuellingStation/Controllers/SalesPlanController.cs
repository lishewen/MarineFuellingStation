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
    public class SalesPlanController : ControllerBase
    {
        private readonly SalesPlanRepository r;
        public SalesPlanController(SalesPlanRepository repository)
        {
            r = repository;
        }

        [HttpGet("[action]")]
        public ResultJSON<string> SalesPlanNo()
        {
            return new ResultJSON<string>
            {
                Code = 0,
                Data = r.GetSerialNumber(r.GetLastSalesPlanNo())
            };
        }
        [HttpPost]
        public ResultJSON<SalesPlan> Post([FromBody]SalesPlan s)
        {
            r.CurrentUser = UserName;
            var result = r.Insert(s);
            return new ResultJSON<SalesPlan>
            {
                Code = 0,
                Data = result
            };
        }
        [HttpGet]
        public ResultJSON<List<SalesPlan>> Get()
        {
            return new ResultJSON<List<SalesPlan>>
            {
                Code = 0,
                Data = r.GetAllList()
            };
        }
    }
}
