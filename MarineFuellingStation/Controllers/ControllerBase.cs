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
    }
}
