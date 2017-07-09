using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MFS.Models;
using Senparc.Weixin.Work.Containers;
using Senparc.Weixin.Work.AdvancedAPIs;
using Microsoft.Extensions.Options;
using System.Net;

namespace MFS.Controllers
{
    public class HomeController : Controller
    {
        WorkOption option;
        public HomeController(IOptionsSnapshot<WorkOption> option)
        {
            this.option = option.Value;
            this.option.AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.Secret);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetOpenId(string redirectUrl)
        {
#if DEBUG
            return Redirect($"/wxhub/{WebUtility.UrlEncode("黄继业")}/{WebUtility.UrlEncode(redirectUrl)}");
#else
            var state = Request.Query["state"];
            if (state != "car0774")
                return Redirect(OAuth2Api.GetCode(option.CorpId, "http://" + Request.Host + Request.Path, "car0774"));
            else
            {
                var code = Request.Query["code"];
                var at = OAuth2Api.GetUserId(option.AccessToken, code);
                var userinfo = MailListApi.GetMember(option.AccessToken, at.UserId);
                return Redirect($"/wxhub/{WebUtility.UrlEncode(userinfo.name)}/{WebUtility.UrlEncode(redirectUrl)}");
            }
#endif
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
