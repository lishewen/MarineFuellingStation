using MFS.Controllers.Attributes;
using MFS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
        /// <summary>
        /// 缓存部门列表
        /// </summary>
        public static List<DepartmentList> DepartmentList { get; set; }
        WorkOption option;
        public UserController(IOptionsSnapshot<WorkOption> option)
        {
            this.option = option.Value;
            this.option.AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.Secret);
        }
    }
}
