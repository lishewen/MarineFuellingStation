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
    public class InAndOutLogController : ControllerBase
    {
        private readonly InAndOutLogRepository r;
        public InAndOutLogController(InAndOutLogRepository repository)
        {
            r = repository;
        }
        #region POST
        [HttpPost]
        public ResultJSON<InAndOutLog> Post([FromBody]InAndOutLog model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<InAndOutLog>
            {
                Code = 0,
                Data = r.Insert(model)
            };
        }
        #endregion
        #region GET
        [HttpGet("[action]")]
        public ResultJSON<List<InAndOutLog>> GetIncludeStore(LogType type = LogType.全部 ,int page = 1)
        {

            return new ResultJSON<List<InAndOutLog>>
            {
                Code = 0,
                Data = r.GetIncludeStore(type,page)
            };
        }
        [HttpGet("{sv}")]
        public ResultJSON<List<InAndOutLog>> Get(string sv)
        {
            return new ResultJSON<List<InAndOutLog>>
            {
                Code = 0,
                Data = r.GetAllList(s => s.Name.Contains(sv))
            };
        }
        #endregion
    }
}
