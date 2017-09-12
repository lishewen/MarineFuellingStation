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
    public class StoreTypeController : ControllerBase
    {
        private readonly StoreTypeRepository r;
        public StoreTypeController(StoreTypeRepository repository)
        {
            r = repository;
        }
        [HttpGet]
        public ResultJSON<List<StoreType>> Get()
        {
            return new ResultJSON<List<StoreType>>
            {
                Code = 0,
                Data = r.GetAllList()
            };
        }
        [HttpGet("{id}")]
        public ResultJSON<StoreType> Get(int id)
        {
            StoreType s = r.Get(id);
            return new ResultJSON<StoreType>
            {
                Code = 0,
                Data = s
            };
        }
        [HttpPut]
        public ResultJSON<StoreType> Put([FromBody]StoreType model)
        {
            return new ResultJSON<StoreType>
            {
                Code = 0,
                Data = r.InsertOrUpdate(model)
            };
        }
        [HttpPost]
        public ResultJSON<StoreType> Post([FromBody]StoreType model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<StoreType>
            {
                Code = 0,
                Data = r.Insert(new StoreType { Name = model.Name })
            };
        }
    }
}
