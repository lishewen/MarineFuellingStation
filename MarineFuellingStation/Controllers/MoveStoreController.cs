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
    public class MoveStoreController : ControllerBase
    {
        private readonly MoveStoreRepository r;
        public MoveStoreController(MoveStoreRepository repository)
        {
            r = repository;
        }
        [HttpGet("[action]")]
        public ResultJSON<string> MoveStoreNo()
        {
            return new ResultJSON<string>
            {
                Code = 0,
                Data = r.GetSerialNumber(r.GetLastMoveStoreNo())
            };
        }
        [HttpGet]
        public ResultJSON<List<Models.GET.MoveStore>> Get(bool isFinished)
        {
            return new ResultJSON<List<Models.GET.MoveStore>>()
            {
                Code = 0,
                Data = r.GetForIsFinished(isFinished)
            };
        }
        [HttpPost]
        public ResultJSON<MoveStore> Post([FromBody]MoveStore m)
        {
            r.CurrentUser = UserName;
            var result = r.Insert(m);

            return new ResultJSON<MoveStore>
            {
                Code = 0,
                Data = result
            };
        }
    }
}
