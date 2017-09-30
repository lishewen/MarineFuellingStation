using MFS.Controllers.Attributes;
using MFS.Models;
using MFS.Repositorys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        /// <summary>
        /// 分页显示数据
        /// </summary>
        /// <param name="page">第N页</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="sv">搜索关键字</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<List<ChargeLog>> GetByPager(int page, int pageSize, string sv = "")
        {
            List<ChargeLog> cl;
            if (string.IsNullOrEmpty(sv))
                cl = r.LoadPageList(page, pageSize, out int rCount, true).Include("Client").OrderByDescending(c => c.Id).ToList();
            else
                cl = r.LoadPageList(page, pageSize, out int rCount, true, c => c.Name.Contains(sv) || c.CompanyName.Contains(sv)).Include("Client").OrderByDescending(c => c.Id).ToList();
            return new ResultJSON<List<ChargeLog>>
            {
                Code = 0,
                Data = cl
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
        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isCompanyCharge">公司充值或客户充值</param>
        /// <returns></returns>
        [HttpPost]
        public ResultJSON<ChargeLog> Post([FromBody]ChargeLog model, bool isCompanyCharge)
        {
            r.CurrentUser = UserName;
            ChargeLog c = r.InsertAndUpdateBalances(model, isCompanyCharge);
            if (c == null)
                return new ResultJSON<ChargeLog>
                {
                    Code = 501,
                    Msg = "无法更新客户余额"
                };
            else
                return new ResultJSON<ChargeLog>
                {
                    Code = 0,
                    Data = c
                };
        }
    }
}
