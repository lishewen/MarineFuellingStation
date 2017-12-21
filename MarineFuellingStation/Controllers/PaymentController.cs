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
    public class PaymentController : ControllerBase
    {
        private readonly PaymentRepository r;
        public PaymentController(PaymentRepository repository)
        {
            r = repository;
        }
        #region POST
        [HttpPost]
        public ResultJSON<Payment> Post([FromBody]Payment model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Payment>
            {
                Code = 0,
                Data = r.Insert(model)
            };
        }
        #endregion
        #region GET
        [HttpGet]
        public ResultJSON<List<Payment>> Get()
        {
            return new ResultJSON<List<Payment>>
            {
                Code = 0,
                Data = r.GetAllList()
            };
        }
        [HttpGet("[action]")]
        public ResultJSON<List<Payment>> GetByOrderId(int oid)
        {
            return new ResultJSON<List<Payment>>
            {
                Code = 0,
                Data = r.GetAllList(p => p.OrderId == oid)
            };
        }
        [HttpGet("[action]")]
        public ResultJSON<List<Payment>> GetByBoatCleanId(int bid)
        {
            return new ResultJSON<List<Payment>>
            {
                Code = 0,
                Data = r.GetAllList(p => p.BoatCleanId == bid)
            };
        }
        [HttpGet("{sv}")]
        public ResultJSON<List<Payment>> Get(string sv)
        {
            return new ResultJSON<List<Payment>>
            {
                Code = 0,
                Data = r.GetAllList(s => s.Name.Contains(sv))
            };
        }
        #endregion
    }
}
