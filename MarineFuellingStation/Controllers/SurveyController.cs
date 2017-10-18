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
    public class SurveyController : ControllerBase
    {
        private readonly SurveyRepository r;
        public SurveyController(SurveyRepository repository)
        {
            r = repository;
        }
        [HttpPost]
        public ResultJSON<Survey> Post([FromBody]Survey model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Survey>
            {
                Code = 0,
                Data = r.Insert(model)
            };
        }
        [HttpGet]
        public ResultJSON<List<Survey>> Get()
        {
            return new ResultJSON<List<Survey>>
            {
                Code = 0,
                Data = r.GetAllList()
            };
        }
        [HttpGet("{sv}")]
        public ResultJSON<List<Survey>> Get(string sv)
        {
            return new ResultJSON<List<Survey>>
            {
                Code = 0,
                Data = r.GetAllList(s => s.Name.Contains(sv))
            };
        }
        [HttpGet("[action]/{stid}")]
        public ResultJSON<List<Survey>> GetTop15(int stid)
        {
            return new ResultJSON<List<Survey>>
            {
                Code = 0,
                Data = r.Top15(stid)
            };
        }
    }
}
