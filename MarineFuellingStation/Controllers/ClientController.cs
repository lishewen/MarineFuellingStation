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
    public class ClientController : ControllerBase
    {
        private readonly ClientRepository r;
        public ClientController(ClientRepository repository)
        {
            r = repository;
        }
        [HttpPost]
        public ResultJSON<Client> Post([FromBody]Client model)
        {
            r.CurrentUser = UserName;
            model.CreatedBy = UserName;
            model.CreatedAt = DateTime.Now;
            return new ResultJSON<Client>
            {
                Code = 0,
                Data = r.Insert(model)
            };
        }
        [HttpGet("{sv}")]
        public ResultJSON<List<Client>> Get(string sv)
        {
            return new ResultJSON<List<Client>>
            {
                Code = 0,
                Data = r.GetAllList(s => s.carNo.Contains(sv) 
                    || s.Company.Name.Contains(sv) 
                    || s.Contact.Contains(sv))
            };
        }
    }
}
