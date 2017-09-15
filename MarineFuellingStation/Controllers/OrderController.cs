using MFS.Controllers.Attributes;
using MFS.Helper;
using MFS.Hubs;
using MFS.Models;
using MFS.Repositorys;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Controllers
{
    [Route("api/[controller]"), Axios]
    public class OrderController : ControllerBase
    {
        private readonly OrderRepository r;
        private readonly IHubContext<PrintHub> _hub;
        private readonly IHostingEnvironment _hostingEnvironment;
        public OrderController(OrderRepository repository, IHubContext<PrintHub> hub, IHostingEnvironment env)
        {
            r = repository;
            _hub = hub;
            _hostingEnvironment = env;
        }
        #region Post方法
        [HttpPost]
        public async Task<ResultJSON<Order>> Post([FromBody]Order o)
        {
            r.CurrentUser = UserName;
            var result = r.Insert(o);

            //推送打印指令
            await _hub.Clients.All.InvokeAsync("printorder", result);

            return new ResultJSON<Order>
            {
                Code = 0,
                Data = result
            };
        }
        [HttpPost("[action]")]
        public async Task<ResultJSON<string>> UploadFile([FromForm]IFormFile file)
        {
            if (file != null)
            {
                var extName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
                int i = extName.LastIndexOf('.');
                extName = extName.Substring(i);
                string fileName = Guid.NewGuid() + extName;
                var filePath = _hostingEnvironment.WebRootPath + @"\upload\" + fileName;
                await file.SaveAsAsync(filePath);
                return new ResultJSON<string>
                {
                    Code = 0,
                    Data = $"/upload/{fileName}"
                };
            }
            else
            {
                return new ResultJSON<string>
                {
                    Code = 1
                };
            }
        }
        #endregion
        #region GET方法

        [HttpGet("[action]")]
        public async Task<ResultJSON<string>> OrderNo()
        {
            await _hub.Clients.All.InvokeAsync("login", UserName);

            return new ResultJSON<string>
            {
                Code = 0,
                Data = r.GetSerialNumber(r.GetLastOrderNo())
            };
        }
        [HttpGet]
        public ResultJSON<List<Order>> Get()
        {
            return new ResultJSON<List<Order>>
            {
                Code = 0,
                Data = r.GetAllList()
            };
        }
        [HttpGet("{id}")]
        public ResultJSON<Order> Get(int id)
        {
            return new ResultJSON<Order>
            {
                Code = 0,
                Data = r.Get(id)
            };
        }
        /// <summary>
        /// 获取水上或陆上分页数据，并且包含Product对象
        /// </summary>
        /// <param name="orderType">水上/陆上/机油</param>
        /// <param name="page">第n页</param>
        /// <returns></returns>
        [HttpGet("[action]/{ordertype}")]
        public ResultJSON<List<Order>> GetIncludeProduct(SalesPlanType orderType, int page)
        {
            return new ResultJSON<List<Order>>
            {
                Code = 0,
                Data = r.GetIncludeProduct(orderType, page, 30)//每页30条记录
            };
        }
        #endregion
        #region Put方法
        /// <summary>
        /// 施工过程切换状态
        /// </summary>
        /// <returns></returns>
        [HttpPut("[action]")]
        public ResultJSON<Order> ChangeState([FromBody]Order o)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Order>
            {
                Code = 0,
                Data = r.Update(o)
            };
        }
        #endregion
    }
}
