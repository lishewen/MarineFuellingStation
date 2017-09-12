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
    public class ProductTypeController : ControllerBase
    {
        private readonly ProductTypeRepository r;
        public ProductTypeController(ProductTypeRepository repository)
        {
            r = repository;
        }
        [HttpGet]
        public ResultJSON<List<ProductType>> Get()
        {
            return new ResultJSON<List<ProductType>>
            {
                Code = 0,
                Data = r.GetAllList()
            };
        }
        [HttpGet("{id}")]
        public ResultJSON<ProductType> Get(int id)
        {
            ProductType s = r.Get(id);
            return new ResultJSON<ProductType>
            {
                Code = 0,
                Data = s
            };
        }
        [HttpPut]
        public ResultJSON<ProductType> Put([FromBody]ProductType model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<ProductType>
            {
                Code = 0,
                Data = r.InsertOrUpdate(model)
            };
        }
        [HttpPost]
        public ResultJSON<ProductType> Post([FromBody]ProductType model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<ProductType>
            {
                Code = 0,
                Data = r.Insert(new ProductType { Name = model.Name })
            };
        }
    }
}
