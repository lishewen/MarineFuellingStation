using MFS.Controllers.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Senparc.Weixin.Work.AdvancedAPIs;
using Microsoft.Extensions.Options;
using MFS.Models;
using Senparc.Weixin.Work.Containers;
using Senparc.Weixin.Work.AdvancedAPIs.MailList;

namespace MFS.Controllers
{
    [Route("api/[controller]"), Axios]
    public class DepartmentController : ControllerBase
    {
        WorkOption option;
        public DepartmentController(IOptionsSnapshot<WorkOption> option)
        {
            this.option = option.Value;
            this.option.AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.Secret);
        }
        [HttpGet]
        public GetDepartmentListResult Get()
        {
            var result = MailListApi.GetDepartmentList(option.AccessToken);
            return result;
        }
    }
}
