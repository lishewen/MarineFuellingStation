using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MFS.Controllers.Attributes;
using MFS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Senparc.Weixin.Work.AdvancedAPIs;
using Senparc.Weixin.Work.AdvancedAPIs.OaDataOpen;
using Senparc.Weixin.Work.Containers;

namespace MFS.Controllers
{
    [Route("api/[controller]"), Axios]
    public class OaController: ControllerBase
    {
        WorkOption option;
        public OaController(IOptionsSnapshot<WorkOption> option)
        {
            this.option = option.Value;
            this.option.Secret = "";
            this.option.AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.打卡Secret);
        }
        [HttpGet("[action]")]
        public GetCheckinDataJsonResult GetUserCheckinData()
        {
            DateTime now = DateTime.Now;
            DateTime start = new DateTime(now.Year, now.Month, 1);
            DateTime end = now;
            string[] userlist = new string[] { "FangJingMin" };
            return OaDataOpenApi.GetCheckinData(this.option.AccessToken, OaDataOpenApi.OpenCheckinDataType.全部打卡, start, end, userlist);
        }
    }
}
