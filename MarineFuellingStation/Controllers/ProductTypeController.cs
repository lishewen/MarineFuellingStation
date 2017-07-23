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
    }
}
