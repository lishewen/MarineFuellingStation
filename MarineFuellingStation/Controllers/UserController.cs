using MFS.Controllers.Attributes;
using MFS.Models;
using MFS.Repositorys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Senparc.Weixin.Work.AdvancedAPIs;
using Senparc.Weixin.Work.AdvancedAPIs.MailList;
using Senparc.Weixin.Work.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Controllers
{
    [Route("api/[controller]"), Axios]
    public class UserController : ControllerBase
    {
        WorkOption option;
        private readonly UserRepository r;
        public UserController(IOptionsSnapshot<WorkOption> option, UserRepository repository)
        {
            r = repository;

            this.option = option.Value;
            this.option.AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.Secret);
        }
        [HttpGet]
        public GetDepartmentMemberInfoResult Get()
        {
            return MailListApi.GetDepartmentMemberInfo(option.AccessToken, 1, 1);
        }
        [HttpGet("{id}")]
        public ResultJSON<UserDTO> Get(string id)
        {
            r.CurrentUser = UserName;

            var result = new UserDTO
            {
                WorkInfo = MailListApi.GetMember(option.AccessToken, id)
            };

            var model = r.FirstOrDefault(u => u.UserId == id);
            if (model == null)
            {
                var user = new User
                {
                    Name = result.WorkInfo.name,
                    UserId = result.WorkInfo.userid,
                    AccountName = result.WorkInfo.name
                };
                model = r.Insert(user);
            }

            result.LocalInfo = model;

            return new ResultJSON<UserDTO>
            {
                Code = 0,
                Data = result
            };
        }

        [HttpPost]
        public ResultJSON<UserDTO> Post([FromBody]UserDTO dto)
        {
            r.CurrentUser = UserName;
            dto.LocalInfo = r.InsertOrUpdate(dto.LocalInfo);

            return new ResultJSON<UserDTO>
            {
                Code = 0,
                Data = dto
            };
        }

        [HttpGet("[action]")]
        public GetTagMemberResult Salesman()
        {
            return GetTagMember("销售");
        }

        [HttpGet("[action]")]
        public GetTagMemberResult Manufacturer()
        {
            return GetTagMember("生成员");
        }

        [NonAction]
        private GetTagMemberResult GetTagMember(string tagname)
        {
            var listresult = MailListApi.GetTagList(option.AccessToken);
            var tagid = listresult.taglist.FirstOrDefault(t => t.tagname == tagname).tagid;
            return MailListApi.GetTagMember(option.AccessToken, Convert.ToInt32(tagid));
        }
    }
}
