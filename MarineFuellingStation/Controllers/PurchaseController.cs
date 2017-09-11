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
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseRepository r;
        public PurchaseController(PurchaseRepository repository)
        {
            r = repository;
        }
        [HttpGet]
        public ResultJSON<List<Purchase>> Get()
        {
            return new ResultJSON<List<Purchase>>
            {
                Code = 0,
                Data = r.GetIncludeProduct()
            };
        }
        [HttpGet("{sv}")]
        public ResultJSON<List<Purchase>> Get(string sv)
        {
            return new ResultJSON<List<Purchase>>
            {
                Code = 0,
                Data = r.GetIncludeProduct().Where(s => s.CarNo.Contains(sv)).ToList()
            };
        }
        [HttpGet("[action]")]
        public ResultJSON<string> PurchaseNo()
        {
            return new ResultJSON<string>
            {
                Code = 0,
                Data = r.GetSerialNumber(r.GetLastPurchaseNo())
            };
        }
        [HttpPost]
        public ResultJSON<Purchase> Post([FromBody]Purchase p)
        {
            r.CurrentUser = UserName;
            var result = r.Insert(p);

            return new ResultJSON<Purchase>
            {
                Code = 0,
                Data = result
            };
        }
    }
}
