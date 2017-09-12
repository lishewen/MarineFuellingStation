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
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository r;
        public ProductController(ProductRepository repository)
        {
            r = repository;
        }
        [HttpGet("[action]")]
        public ResultJSON<List<Product>> OilProducts()
        {
            return new ResultJSON<List<Product>>
            {
                Code = 0,
                Data = r.GetAllList((p) => p.ProductType.Name == "销售油" && p.IsUse)
            };
        }
        [HttpGet("{id}")]
        public ResultJSON<Product> Get(int id)
        {
            Product s = r.Get(id);
            return new ResultJSON<Product>
            {
                Code = 0,
                Data = s
            };
        }
        [HttpPut]
        public ResultJSON<Product> Put([FromBody]Product model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Product>
            {
                Code = 0,
                Data = r.InsertOrUpdate(model)
            };
        }
        /// <summary>
        /// 新增POST
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ResultJSON<Product> Post([FromBody]Product model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Product>
            {
                Code = 0,
                Data = r.Insert(model)
            };
        }
    }
}
