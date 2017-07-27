using MFS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MFS.Controllers
{
    public class ControllerBase : Controller
    {
        public EFContext db;
        public string UserName
        {
            get
            {
                return HttpContext.Session.GetString("UserName");
            }
            set
            {
                string username = value ?? string.Empty;
                username = WebUtility.UrlDecode(username);
                HttpContext.Session.SetString("UserName", username);
                if (db != null)
                    db.CurrentUser = username;

            }
        }
        public string UserId
        {
            get
            {
                return HttpContext.Session.GetString("UserId");
            }
            set
            {
                string userid = value ?? string.Empty;
                userid = WebUtility.UrlDecode(userid);
                HttpContext.Session.SetString("UserId", userid);
            }
        }
    }
}
