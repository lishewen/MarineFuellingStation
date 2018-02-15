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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Senparc.Weixin.Work.Containers;
using Senparc.Weixin.Work.AdvancedAPIs;
using System.IO;

namespace MFS.Controllers
{
    [Route("api/[controller]"), Axios]
    public class OrderController : ControllerBase
    {
        private readonly OrderRepository r;
        private readonly ClientRepository cr;
        private readonly IHubContext<PrintHub> _hub;
        private readonly IHostingEnvironment _hostingEnvironment;
        WorkOption option;
        public OrderController(OrderRepository repository, IHubContext<PrintHub> hub, IOptionsSnapshot<WorkOption> option, IHostingEnvironment env, ClientRepository clientRepository)
        {
            r = repository;
            cr = clientRepository;
            _hub = hub;
            _hostingEnvironment = env;
            //获取 销售单 企业微信应用的AccessToken
            this.option = option.Value;
        }
        #region 推送打印指令到指定打印机端
        [NonAction]
        public async Task SendPrintAsync(string who, Order order, string actionName)
        {
            foreach (var connectionId in PrintHub.connections.GetConnections(who))
            {
                await _hub.Clients.Client(connectionId).InvokeAsync(actionName, order);
            }
        }
        #endregion
        #region Post方法
        [HttpPost]
        public async Task<ResultJSON<Order>> Post([FromBody]Order o)
        {
            //判断是否重复单号
            if (r.Has(od => od.Name == o.Name))
                return new ResultJSON<Order> { Code = 502 };

            r.CurrentUser = UserName;
            
            if (r.Has(od => od.Name == o.Name)) return new ResultJSON<Order> { Code = 501, Msg = "已存在单号" + o.Name + ",请勿重复提交" };

            //如果不存在该客户，则新增到Client表中，并关联ClientId
            if (!cr.Has(cl => cl.CarNo == o.CarNo)) {
                Client c = cr.Insert(new Client { Name = "个人", CarNo = o.CarNo});
                o.ClientId = c.Id;
            }

            //如果没有计划，则不用指定销售员，客户需求，不用计算提成
            if(!o.SalesPlanId.HasValue)
                o.Salesman = "";

            //标识“陆上”和“水上”的单
            o.IsWater = o.OrderType == SalesPlanType.水上加油 || o.OrderType == SalesPlanType.水上机油 ? true : false;

            var result = r.Insert(o);

            //"水上加油"不再独立施工流程，跳过施工过程直接“完工”状态
            if(o.OrderType == SalesPlanType.水上加油) {
                o.State = OrderState.已完成;
                o.OilCountLitre = o.Count;
                o.OilCount = o.Count;
                var res  = r.ChangeState(o);
                //推送到“油仓情况”
                this.option.油仓情况AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.油仓情况Secret);
                await MassApi.SendTextCardAsync(option.油仓情况AccessToken, option.油仓情况AgentId, $"{result.CarNo}加油完工，已更新油仓油量"
                         , $"<div class=\"gray\">单号：{result.Name}</div>" +
                         $"<div class=\"normal\">施工人：{result.Worker}</div>" +
                         $"<div class=\"normal\">数量：{Math.Round(result.OilCountLitre, 2)}升</div>"
                         , $"https://vue.car0774.com/#/sales/order/{result.Id}/order", toUser: "@all");
            }

            //推送打印指令
            //await _hub.Clients.All.InvokeAsync("printorder", result);
            
            //初始化推送需要到的AccessToken
            this.option.销售单AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.销售单Secret);
            this.option.加油AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.加油Secret);

            //#if !DEBUG

            //推送到“收银”
            this.option.收银AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.收银Secret);
            await MassApi.SendTextCardAsync(option.收银AccessToken, option.收银AgentId, "已开单"
                     , $"<div class=\"gray\">单号：{result.Name}</div>" +
                     $"<div class=\"normal\">开单人：{UserName}</div>" +
                     $"<div class=\"normal\">船号/车号/客户名称：{result.CarNo}</div>"
                     , $"https://vue.car0774.com/#/sales/order/{result.Id}/order", toUser: "@all");

            string strType = "",
                orderUrl = "",
                produceUrl = "",
                carOrBoat = "";
            switch (result.OrderType)
            {
                case SalesPlanType.水上加油:
                    strType = "水上加油";
                    orderUrl = $"https://vue.car0774.com/#/sales/order/{result.Id}/order";
                    produceUrl = $"https://vue.car0774.com/#/produce/load/{result.Id}/0";
                    carOrBoat = "船号";
                    break;
                case SalesPlanType.陆上装车:
                    strType = "陆上装车";
                    orderUrl = $"https://vue.car0774.com/#/sales/order/{result.Id}/order";
                    produceUrl = $"https://vue.car0774.com/#/produce/landload/{result.Id}";
                    carOrBoat = "车号/客户名";
                    break;
                case SalesPlanType.汇鸿车辆加油:
                    strType = "汇鸿车辆加油";
                    orderUrl = $"https://vue.car0774.com/#/sales/order/{result.Id}/order";
                    produceUrl = $"https://vue.car0774.com/#/produce/load/{result.Id}/4";
                    carOrBoat = "车号/客户名";
                    break;
                case SalesPlanType.外来车辆加油:
                    strType = "外来车加油";
                    orderUrl = $"https://vue.car0774.com/#/sales/order/{result.Id}/order";
                    produceUrl = $"https://vue.car0774.com/#/produce/load/{result.Id}/5";
                    carOrBoat = "车号/客户名";
                    break;
            }

            //推送到“销售单”
            await MassApi.SendTextCardAsync(option.销售单AccessToken, option.销售单AgentId, $"【{strType}】{UserName}开了销售单"
                     , $"<div class=\"gray\">单号：{result.Name}</div>" +
                     $"<div class=\"normal\">{carOrBoat}：{result.CarNo}</div>"
                     , orderUrl, toUser: "@all");

            if (result.OrderType != SalesPlanType.水上机油
                && result.OrderType != SalesPlanType.水上加油)
            {
                //推送到“加油”施工
                await MassApi.SendTextCardAsync(option.加油AccessToken, option.加油AgentId, $"{strType}，请施工"
                         , $"<div class=\"gray\">单号：{result.Name}</div>" +
                         $"<div class=\"normal\">开单人：{UserName}</div>" +
                         $"<div class=\"normal\">{carOrBoat}：{result.CarNo}</div>"
                         , produceUrl, toUser: "@all");
            }
            //#endif
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
                Data = r.GetWithInclude(1, 30)
            };
        }
        [HttpGet("{id}")]
        public ResultJSON<Order> Get(int id)
        {
            return new ResultJSON<Order>
            {
                Code = 0,
                Data = r.GetWithInclude(id)
            };
        }
        /// <summary>
        /// 分页显示数据
        /// </summary>
        /// <param name="page">第N页</param>
        /// <param name="pageSize">页记录数</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<List<Order>> GetByPager(int page, int pageSize, bool isWater, string sv = "")
        {
            List<Order> list;
            if (string.IsNullOrEmpty(sv))
                list = r.LoadPageList(page, pageSize, out int rCount, true, false, o => o.IsWater == isWater).Include(o => o.Product).OrderByDescending(s => s.Id).ToList();
            else
                list = r.LoadPageList(page, pageSize, out int rCount, true, false, o => o.CarNo.Contains(sv) && o.IsWater == isWater).Include(o => o.Product).OrderByDescending(s => s.Id).ToList();
            return new ResultJSON<List<Order>>
            {
                Code = 0,
                Data = list
            };
        }
        /// <summary>
        /// 获取我的订单分页数据，并且包含Product对象
        /// </summary>
        /// <param name="orderType">水上/陆上/机油</param>
        /// <param name="page">第n页</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<List<Order>> GetMyOrders(int page, int size, DateTime startDate, DateTime endDate)
        {
            return new ResultJSON<List<Order>>
            {
                Code = 0,
                Data = r.LoadPageList(page, size, out int cnt, true, false, o => o.Salesman == UserName && o.CreatedAt >= startDate && o.CreatedAt < endDate).ToList()
            };
        }
        /// <summary>
        /// 获取我的订单分页数据，并且包含Product对象
        /// </summary>
        /// <param name="orderType">水上/陆上/机油</param>
        /// <param name="page">第n页</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<List<Order>> GetOrders(int page, int size, DateTime startDate, DateTime endDate, string sales = "")
        {
            List<Order> list = new List<Order>();
            if (string.IsNullOrEmpty(sales))
                list = r.LoadPageList(page, size, out int cnt, true, false, o => o.CreatedAt >= startDate && o.CreatedAt < endDate).ToList();
            else
                list = r.LoadPageList(page, size, out int cnt, true, false, o => o.Salesman == sales && o.CreatedAt >= startDate && o.CreatedAt < endDate).ToList();
            return new ResultJSON<List<Order>>
            {
                Code = 0,
                Data = list
            };
        }
        /// <summary>
        /// 获取所有记录分页数据，并且包含Product对象
        /// </summary>
        /// <param name="page">第n页</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<List<Order>> GetIncludeProduct(int page)
        {
            return new ResultJSON<List<Order>>
            {
                Code = 0,
                Data = r.GetWithInclude(page, 30)//每页30条记录
            };
        }
        /// <summary>
        /// 根据是否施工完成获取记录分页数据，并且包含Product对象
        /// </summary>
        /// <param name="page">第n页</param>
        /// <param name="isFinished">是否完成状态</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<List<Order>> GetByIsFinished(int page, bool isFinished)
        {
            return new ResultJSON<List<Order>>
            {
                Code = 0,
                Data = r.GetIncludeProduct(page, 30, isFinished)//每页30条记录
            };
        }
        /// <summary>
        /// 根据是否完工获取水上或陆上分页数据，并且包含Product对象
        /// </summary>
        /// <param name="orderType">水上/陆上/机油</param>
        /// <param name="page">第n页</param>
        /// <param name="isFinished">是否完成状态</param>
        /// <returns></returns>
        [HttpGet("[action]/{ordertype}")]
        public ResultJSON<List<Order>> GetByIsFinished(SalesPlanType orderType, int page, bool isFinished)
        {
            return new ResultJSON<List<Order>>
            {
                Code = 0,
                Data = r.GetIncludeProduct(orderType, page, 30, isFinished)//每页30条记录
            };
        }
        /// <summary>
        /// 根据付款状态获取分页数据，并且包含Product、Client对象
        /// </summary>
        /// <param name="payState">付款状态</param>
        /// <param name="page">第n页</param>
        /// <param name="pageSize">分页记录数</param>
        /// <param name="searchVal">搜索关键字</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<List<Order>> GetByPayState(PayState payState, int page, int pageSize, string searchVal = "")
        {
            return new ResultJSON<List<Order>>
            {
                Code = 0,
                Data = r.GetByPayState(payState, page, pageSize, searchVal)//每页30条记录
            };
        }
        [HttpGet("[action]/{ordertype}")]
        public ResultJSON<Order> GetLastOrder(SalesPlanType ordertype)
        {
            Order order = r.GetLastOrder(ordertype);
            return new ResultJSON<Order>
            {
                Code = 0,
                Data = order
            };
        }
        /// <summary>
        /// 向指定打印机推送【调拨单】打印指令
        /// </summary>
        /// <param name="id">Order id</param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ResultJSON<Order>> PrintOrder(int id, string to)
        {
            Order o = r.GetWithInclude(id);
            await SendPrintAsync(to, o, "printorder");
            return new ResultJSON<Order>
            {
                Code = 0,
                Data = o
            };
        }
        /// <summary>
        /// 向指定打印机推送【陆上装车单】打印指令
        /// </summary>
        /// <param name="id">Order id</param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ResultJSON<Order>> PrintLandload(int id, string to)
        {
            Order o = r.GetWithInclude(id);
            await SendPrintAsync(to, o, "printlandload");
            return new ResultJSON<Order>
            {
                Code = 0,
                Data = o
            };
        }
        /// <summary>
        /// 向指定打印机推送陆上【送货单】打印指令
        /// </summary>
        /// <param name="id">Order id</param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ResultJSON<Order>> PrintDeliver(int id, string to)
        {
            Order o = r.Get(id);
            await SendPrintAsync(to, o, "printdeliver");
            return new ResultJSON<Order>
            {
                Code = 0,
                Data = o
            };
        }
        /// <summary>
        /// 向指定打印机推送陆上【预收款单】打印指令
        /// </summary>
        /// <param name="id">Order id</param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ResultJSON<Order>> PrintPonderation(int id, string to)
        {
            Order o = r.Get(id);
            await SendPrintAsync(to, o, "printponderation");
            return new ResultJSON<Order>
            {
                Code = 0,
                Data = o
            };
        }
        /// <summary>
        /// 根据ClientID获得所有挂账金额
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<decimal> GetClientNoPayMoney(int cid)
        {
            decimal sumNopay = r.GetSumNoPay(cid);
            return new ResultJSON<decimal>
            {
                Code = 0,
                Data = sumNopay
            };
        }
        /// <summary>
        /// 根据CompanyID获得所有公司挂账金额
        /// </summary>
        /// <param name="id">Company ID</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<decimal> GetCompanyNoPayMoney(int coId)
        {
            decimal sumNopay = r.GetSumNoPayByCoId(coId);
            return new ResultJSON<decimal>
            {
                Code = 0,
                Data = sumNopay
            };
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ResultJSON<string>> ExportExcel(DateTime start, DateTime end)
        {
            try
            {
                List<Order> list = r.GetOrdersForExportExcel(start, end);
                if (list == null || list.Count == 0)
                    return new ResultJSON<string> { Code = 503, Msg = "没有相关数据" };

                var excellist = new List<OrderExcel>();
                OrderExcel oe;
                #region 赋值到excel model
                foreach (var item in list)
                {
                    oe = new OrderExcel
                    {
                        陆上或水上 = item.IsWater? "水上" : "陆上",
                        单号 = item.Name,
                        船号或车号 = item.CarNo,
                        计划单号 = item.Name,
                        客户 = item.Client == null ? "" : item.Client.CarNo,
                        销售员 = item.Salesman,
                        商品 = item.Product == null ? "" : item.Product.Name,
                        当时最低单价 = item.MinPrice,
                        单价 = item.Price,
                        数量 = item.Count,
                        金额 = item.TotalMoney,
                        是否开票 = item.IsInvoice ? "开票" : "",
                        运费 = item.DeliverMoney,
                        开票公司 = item.BillingCompany,
                        开票类型 = Enum.GetName(typeof(TicketType), item.TicketType),
                        开票单价 = item.BillingPrice,
                        开票数量 = item.BillingCount,
                        销售仓 = item.Store == null ? "" : item.Store.Name,
                        实际加油数量 = item.OilCountLitre,
                        生产员 = item.Worker,
                        密度 = item.Density,
                        油温 = item.OilTemperature,
                        毛重 = item.OilCarWeight,
                        皮重 = item.EmptyCarWeight,
                        销售超额提成 = item.SalesCommission,
                        支付状态 = Enum.GetName(typeof(PayState), item.PayState),
                        收银员 = item.Cashier,

                        创建时间 = item.CreatedAt.ToString("yyyy-MM-dd hh:mm"),
                        备注 = item.Remark
                    };
                    excellist.Add(oe);
                }
                #endregion
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, @"excel\");
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_销售单.xlsx";
                Helper.FileHelper.ExportExcelByEPPlus(excellist, filePath + fileName);
                string filePathURL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, @"excel/" + fileName);

                //推送到“导出数据”
                this.option.导出数据AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.导出数据Secret);
                await MassApi.SendTextCardAsync(option.导出数据AccessToken, option.导出数据AgentId, $"{UserName}导出销售单数据到Excel"
                         , $"<div class=\"gray\">操作时间：{DateTime.Now.ToString()}</div>"
                          + $"<div class=\"gray\">导出时间段：{start.ToString()} - {end.ToString()}</div>"
                         , filePathURL, toUser: "@all");

                return new ResultJSON<string> { Code = 0, Data = filePathURL };
            }
            catch (Exception e)
            {
                return new ResultJSON<string> { Code = 503, Msg = e.Message };
            }
        }
        #endregion
        #region Put方法
        /// <summary>
        /// 施工过程切换状态
        /// </summary>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<ResultJSON<Order>> ChangeState([FromBody]Order o)
        {
            r.CurrentUser = UserName;
            Order result = r.ChangeState(o);

            if(o.State == OrderState.已完成)
            {
                //推送到“油仓情况”
                this.option.油仓情况AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.油仓情况Secret);
                await MassApi.SendTextCardAsync(option.油仓情况AccessToken, option.油仓情况AgentId, $"{result.CarNo}加油完工，已更新油仓油量"
                         , $"<div class=\"gray\">单号：{result.Name}</div>" +
                         $"<div class=\"normal\">施工人：{result.Worker}</div>" +
                         $"<div class=\"normal\">数量：{Math.Round(result.OilCountLitre, 2)}升</div>"
                         , $"https://vue.car0774.com/#/sales/order/{result.Id}/order", toUser: "@all");
            }

            return new ResultJSON<Order>
            {
                Code = 0,
                Data = result
            };
        }
        /// <summary>
        /// 订单结算
        /// </summary>
        /// <param name="model">包含需要更改内容的model</param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public ResultJSON<Order> Pay([FromBody] Order model)
        {
            r.CurrentUser = UserName;
            return r.Pay(model);
        }

        /// <summary>
        /// 挂账
        /// </summary>
        /// <param name="model">包含需要更改内容的model</param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public ResultJSON<Order> PayOnCredit([FromBody]Order model)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Order>
            {
                Code = 0,
                Data = r.ChangePayState(model)
            };
        }
        /// <summary>
        /// 重新施工
        /// </summary>
        /// <param name="oid">订单 id</param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public ResultJSON<Order> Restart([FromBody]Order order)
        {
            r.CurrentUser = UserName;
            var o = r.Restart(order);
            return new ResultJSON<Order>
            {
                Code = 0,
                Data = o
            };
        }
        #endregion
        /// <summary>
        /// 作废单据
        /// </summary>
        /// <param name="id">单据id</param>
        /// <param name="delreason">作废原因</param>
        /// <returns></returns>
        #region Delete
        [HttpDelete]
        public ResultJSON<Order> Del(int id, string delreason)
        {
            try
            {
                Order order = r.SetIsDel(id, delreason);
                return new ResultJSON<Order> { Code = 0, Data = order };
            }
            catch(Exception e)
            {
                return new ResultJSON<Order> { Code = 503, Msg = e.Message };
            }
        }
        #endregion
    }
}
