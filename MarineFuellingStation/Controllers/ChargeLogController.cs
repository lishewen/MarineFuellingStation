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
    public class ChargeLogController : ControllerBase
    {
        private readonly ChargeLogRepository r;
        public ChargeLogController(ChargeLogRepository repository)
        {
            r = repository;
        }
        [HttpGet]
        public ResultJSON<List<ChargeLog>> Get()
        {
            return new ResultJSON<List<ChargeLog>>
            {
                Code = 0,
                Data = r.GetAllList()
            };
        }
        [HttpGet("{id}")]
        public ResultJSON<ChargeLog> Get(int id)
        {
            ChargeLog s = r.Get(id);
            return new ResultJSON<ChargeLog>
            {
                Code = 0,
                Data = s
            };
        }
        [HttpPut]
        public ResultJSON<ChargeLog> Put([FromBody]ChargeLog model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<ChargeLog>
            {
                Code = 0,
                Data = r.InsertOrUpdate(model)
            };
        }
        [HttpPost]
        public ResultJSON<ChargeLog> Post([FromBody]ChargeLog model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<ChargeLog>
            {
                Code = 0,
                Data = r.Insert(new ChargeLog { Name = model.Name })
            };
        }
        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cid">客户ID</param>
        /// <returns></returns>
        [HttpPost]
        public ResultJSON<ChargeLog> Post([FromBody]ChargeLog model, int cid)
        {
            r.CurrentUser = UserName;
            ChargeLog c = r.InsertAndUpdateClient(model, cid);
            return new ResultJSON<ChargeLog>
            {
                Code = 0,
                Data = c
            };
        }
    }
}
