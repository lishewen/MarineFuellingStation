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
                Data = r.GetAllList((p) => p.ProductType.Name == "油品" && p.IsUse)
            };
        }
    }
}
