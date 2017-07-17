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
        public string UserName
        {
            get
            {
                return HttpContext.Session.GetString("UserName");
            }
            set
            {
                string username = value ?? string.Empty;
                HttpContext.Session.SetString("UserName", WebUtility.UrlDecode(username));
            }
        }
    }
}
