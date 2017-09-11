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
    public class BoatCleanController:ControllerBase
    {
        private readonly BoatCleanRepository r;
        public BoatCleanController(BoatCleanRepository repository)
        {
            r = repository;
        }
        [HttpGet("[action]")]
        public ResultJSON<string> BoatCleanNo()
        {
            return new ResultJSON<string>
            {
                Code = 0,
                Data = r.GetSerialNumber(r.GetLastBoatCleanNo())
            };
        }
        [HttpPost]
        public ResultJSON<BoatClean> Post([FromBody]BoatClean b)
        {
            r.CurrentUser = UserName;
            var result = r.Insert(b);

            return new ResultJSON<BoatClean>
            {
                Code = 0,
                Data = result
            };
        }
    }
}
