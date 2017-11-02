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
        private readonly PurchaseRepository pu_r;
        public AssayController(AssayRepository repository, PurchaseRepository pu_repository)
        {
            r = repository;
            pu_r = pu_repository;
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

            if (a.PurchaseId.HasValue)
            {
                var purchase = pu_r.Get(int.Parse(a.PurchaseId.ToString()));
                purchase.AssayId = result.Id;
                pu_r.Update(purchase);
            }
            
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
                Data = r.GetAllList().OrderByDescending(a => a.Id).ToList()
            };
        }
        [HttpGet("[action]")]
        public ResultJSON<List<Assay>> GetWithStANDPur()
        {
            return new ResultJSON<List<Assay>>
            {
                Code = 0,
                Data = r.GetAllWithStANDPur().OrderByDescending(a => a.Id).ToList()
            };
        }
        [HttpGet("[action]/{sId}")]
        public ResultJSON<List<Assay>> GetByStoreId(int sId)
        {
            return new ResultJSON<List<Assay>>
            {
                Code = 0,
                Data = r.GetAllList(a => a.StoreId == sId).OrderByDescending(a => a.Id).ToList()
            };
        }
        [HttpGet("{sv}")]
        public ResultJSON<List<Assay>> Get(string sv)
        {
            return new ResultJSON<List<Assay>>
            {
                Code = 0,
                Data = r.GetAllWithStANDPur(sv).OrderByDescending(a => a.Id).ToList()
            };
        }
    }
}
