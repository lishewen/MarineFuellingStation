using MFS.Controllers.Attributes;
using MFS.Hubs;
using MFS.Models;
using MFS.Repositorys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Controllers
{
    [Route("api/[controller]"), Axios]
    public class AssayController : ControllerBase
    {
        private readonly AssayRepository r;
        private readonly PurchaseRepository pu_r;
        private readonly IHubContext<PrintHub> _hub;
        public AssayController(AssayRepository repository, PurchaseRepository pu_repository, IHubContext<PrintHub> hub)
        {
            r = repository;
            pu_r = pu_repository;
            _hub = hub;
        }
        #region 推送打印指令到指定打印机端
        [NonAction]
        public async Task SendPrintAsync(string who, Assay assay, string actionName)
        {
            foreach (var connectionId in PrintHub.connections.GetConnections(who))
            {
                await _hub.Clients.Client(connectionId).InvokeAsync(actionName, assay);
            }
        }
        #endregion
        [HttpGet("[action]")]
        public ResultJSON<string> AssayNo()
        {
            return new ResultJSON<string>
            {
                Code = 0,
                Data = r.GetSerialNumber(r.GetLastAssayNo())
            };
        }
        [HttpPost]
        public ResultJSON<Assay> Post([FromBody]Assay a)
        {
            //判断是否重复单号
            if (r.Has(ass => ass.Name == a.Name))
                return new ResultJSON<Assay> { Code = 502 };

            r.CurrentUser = UserName;
            a.Assayer = UserName;
            var result = r.Insert(a);

            if (a.PurchaseId.HasValue)
            {
                var purchase = pu_r.Get(int.Parse(a.PurchaseId.ToString()));
                purchase.AssayId = result.Id;
                pu_r.Update(purchase);
            }
            
            return new ResultJSON<Assay>
            {
                Code = 0,
                Data = result
            };
        }
        [HttpGet]
        public ResultJSON<List<Assay>> Get()
        {
            return new ResultJSON<List<Assay>>
            {
                Code = 0,
                Data = r.GetAllList().OrderByDescending(a => a.Id).ToList()
            };
        }
        [HttpGet("[action]")]
        public ResultJSON<List<Assay>> GetByPager(int page, int pageSize)
        {
            return new ResultJSON<List<Assay>>
            {
                Code = 0,
                Data = r.LoadPageList(page, pageSize, out int rowCount).Include(a => a.Store).Include(a => a.Purchase).OrderByDescending(a => a.Id).ToList()
            };
        }
        [HttpGet("[action]/{sId}")]
        public ResultJSON<List<Assay>> GetByStoreId(int sId)
        {
            return new ResultJSON<List<Assay>>
            {
                Code = 0,
                Data = r.GetAllList(a => a.StoreId == sId).OrderByDescending(a => a.Id).ToList()
            };
        }
        [HttpGet("{sv}")]
        public ResultJSON<List<Assay>> Get(string sv)
        {
            return new ResultJSON<List<Assay>>
            {
                Code = 0,
                Data = r.GetWithInclude(sv).OrderByDescending(a => a.Id).ToList()
            };
        }
        /// <summary>
        /// 向指定打印机推送陆上【化验单】打印指令
        /// </summary>
        /// <param name="id">Order id</param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ResultJSON<Assay>> PrintAssay(int id, string to)
        {
            Assay a = r.GetWithInclude(id);
            await SendPrintAsync(to, a, "printassay");
            return new ResultJSON<Assay>
            {
                Code = 0,
                Data = a
            };
        }
    }
}
