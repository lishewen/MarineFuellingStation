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
using MFS.Repositorys;

namespace MFS.Controllers
{
    public class HomeController : ControllerBase
    {
        WorkOption option;
        ProductRepository r;
        public HomeController(IOptionsSnapshot<WorkOption> option, ProductRepository productRepository)
        {
            this.option = option.Value;
            r = productRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetOpenId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                id = "/";
#if DEBUG
            return Redirect($"/#/wxhub/{WebUtility.UrlEncode("黄继业")}/{WebUtility.UrlEncode("13907741118")}/{true}/{1}/{WebUtility.UrlEncode(id)}");
#else
            if (!string.IsNullOrWhiteSpace(UserName) 
                && !string.IsNullOrWhiteSpace(UserId) 
                && !string.IsNullOrWhiteSpace(IsAdmin)
                && !string.IsNullOrWhiteSpace(IsLeader))
                return Redirect($"/#/wxhub/{WebUtility.UrlEncode(UserName)}/{WebUtility.UrlEncode(UserId)}/{IsAdmin}/{IsLeader}/{WebUtility.UrlEncode(id)}");

            var state = Request.Query["state"];
            if (state != "car0774")
                return Redirect(OAuth2Api.GetCode(option.CorpId, "https://" + Request.Host + Request.Path + Request.QueryString, "car0774",""));
            else
            {
                option.AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.Secret);
                var code = Request.Query["code"];
                var at = OAuth2Api.GetUserId(option.AccessToken, code);
                var userinfo = MailListApi.GetMember(option.AccessToken, at.UserId);
                UserName = userinfo.name;
                UserId = at.UserId;
                IsAdmin = userinfo.department.Contains(7).ToString();
                IsLeader = userinfo.isleader.ToString();
                //超级管理员的部门id为7
                return Redirect($"/#/wxhub/{WebUtility.UrlEncode(userinfo.name)}/{WebUtility.UrlEncode(at.UserId)}/{userinfo.department.Contains(7)}/{userinfo.isleader}/{WebUtility.UrlEncode(id)}");
            }
#endif
        }
        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Init()
        {
            //r.Init();
            return Content("OK");
        }
    }
}
