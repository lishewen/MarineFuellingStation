using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MFS.Helper;

namespace MFS.Repositorys
{
    public class OrderRepository : RepositoryBase<Order>
    {
        const string tag = "XS";
        private readonly ChargeLogRepository cl_r;
        public OrderRepository(EFContext dbContext, ChargeLogRepository r) : base(dbContext)
        {
            cl_r = r;
        }

        public string GetLastOrderNo()
        {
            try
            {
                return GetAllList().OrderByDescending(o => o.Id).First().Name;
            }
            catch
            {
                return string.Empty;
            }
        }
        public string GetSerialNumber(string serialNumber = "0")
        {
            if (!string.IsNullOrWhiteSpace(serialNumber) && serialNumber != "0")
            {
                if (serialNumber.Length == 10)
                {
                    string headDate = serialNumber.Substring(2, 4);
                    int lastNumber = int.Parse(serialNumber.Substring(6));
                    //如果数据库最大值流水号中日期和生成日期在同一天，则顺序号加1
                    if (headDate == DateTime.Now.ToString("yyMM"))
                    {
                        lastNumber++;
                        return tag + headDate + lastNumber.ToString("0000");
                    }
                }
            }
            return tag + DateTime.Now.ToString("yyMM") + "0001";
        }
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public new Order Insert(Order entity, bool autoSave = true)
        {
            //根据船号或车号取得clientId
            if (!entity.ClientId.HasValue) { 
                var client = _dbContext.Clients.FirstOrDefault(c => c.CarNo == entity.CarNo);
                if (client != null)
                    entity.ClientId = client.Id;
            }

            //非开票情况过滤关于开票的字段
            if (!entity.IsInvoice)
            {
                entity.BillingCompany = "";
                entity.BillingCount = 0;
                entity.BillingPrice = 0;
            }
            return base.Insert(entity, autoSave);
        }
        public Order GetWithInclude(int id)
        {
            return LoadPageList(1, 1, out int rowCount,true, od => od.Id == id)
                .Include(od => od.Product)
                .Include(od => od.Store)
                .Include(od => od.SalesPlan)
                .Include(od => od.Client)
                .Include(od => od.Client.Company)
                .Include(od => od.Payments)
                .FirstOrDefault();
        }
        /// <summary>
        /// 根据订单类型获取所有订单列表
        /// </summary>
        /// <param name="orderType">订单类型，水上/陆上/机油</param>
        /// <param name="startPage">第N页</param>
        /// <param name="pageSize">每页记录</param>
        /// <returns></returns>
        public List<Order> GetIncludeProduct(SalesPlanType orderType, int startPage, int pageSize)
        {
            return LoadPageList(startPage, pageSize, out int count, true, (o => o.OrderType == orderType)).Include(o => o.Product).ToList();
        }
        /// <summary>
        /// 获取所有订单列表
        /// </summary>
        /// <param name="startPage">第N页</param>
        /// <param name="pageSize">每页记录</param>
        /// <returns></returns>
        public List<Order> GetWithInclude(int startPage, int pageSize)
        {
            return LoadPageList(startPage, pageSize, out int count, true)
                .Include(o => o.Product)
                .Include(o => o.Client)
                .ToList();
        }
        /// <summary>
        /// 获取是否完工状态的所有订单列表
        /// </summary>
        /// <param name="startPage">第N页</param>
        /// <param name="pageSize">每页记录</param>
        /// <param name="isFinished">是否已施工完成</param>
        /// <returns></returns>
        public List<Order> GetIncludeProduct(int startPage, int pageSize, bool isFinished)
        {
            if (isFinished)
                return LoadPageList(startPage, pageSize, out int count, true, o => o.State == OrderState.已完成).Include(o => o.Product).Include(o => o.Client).ToList();
            else
                return LoadPageList(startPage, pageSize, out int count, true, o => o.State != OrderState.已完成).Include(o => o.Product).Include(o => o.Client).ToList();
        }
        /// <summary>
        /// 获取是否完工状态的获取水上或陆上订单列表
        /// </summary>
        /// <param name="startPage">第N页</param>
        /// <param name="pageSize">每页记录</param>
        /// <param name="isFinished">是否已施工完成</param>
        /// <returns></returns>
        public List<Order> GetIncludeProduct(SalesPlanType orderType, int startPage, int pageSize, bool isFinished)
        {
            if (isFinished)
                return LoadPageList(startPage, pageSize, out int count, true, o => o.State == OrderState.已完成 && o.OrderType == orderType).Include(o => o.Product).Include(o => o.Client).ToList();
            else
                return LoadPageList(startPage, pageSize, out int count, true, o => o.State != OrderState.已完成 && o.OrderType == orderType).Include(o => o.Product).Include(o => o.Client).ToList();
        }
        /// <summary>
        /// 根据PayState获取分页数据，并且包含Product、Client、Client.Company对象
        /// </summary>
        /// <param name="payState">付款状态</param>
        /// <param name="startPage">第N页</param>
        /// <param name="pageSize">每页记录</param>
        /// <returns></returns>
        public List<Order> GetByPayState(PayState payState, int startPage, int pageSize, string searchVal)
        {
            Expression<Func<Order, bool>> orderwhere = o => o.PayState == payState;
            if (!string.IsNullOrEmpty(searchVal))
                orderwhere = orderwhere.And(o => o.Name.Contains(searchVal) || o.CarNo.Contains(searchVal));

            return LoadPageList(startPage, pageSize, out int count, true, orderwhere)
                .Include(o => o.Product).Include(o => o.Client).Include(o => o.Client.Company).Include(o => o.Payments).OrderByDescending(o => o.LastUpdatedAt).ToList();
        }
        /// <summary>
        /// 结算订单
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns></returns>
        public ResultJSON<Order> Pay(Order model)
        {
            ResultJSON<Order> ret = new ResultJSON<Order> { Code = 0 };
            Order o = _dbContext.Orders.Find(model.Id);
            o.PayState = model.PayState;
            o.Cashier = CurrentUser;

            //计算订单销售提成
            if (o.PayState == PayState.已结算)
            {
                switch (o.Unit)
                {
                    case "升":
                        o.SalesCommission = (o.Price - o.MinPrice) * o.Count * 0.2M / 1200;
                        break;
                    case "吨":
                        o.SalesCommission = (o.Price - o.MinPrice) * o.Count * 0.2M;
                        break;
                    default:
                        break;
                }
            }
            //新增付款记录Payment
            foreach (Payment p in model.Payments)
            {
                //要加载order下才会关联OrderId，直接加在db下不会反应关联关系
                o.Payments.Add(p);
                if (p.PayTypeId == OrderPayType.账户扣减 || p.PayTypeId == OrderPayType.公司账户扣减)
                {
                    bool isCompany = (p.PayTypeId == OrderPayType.公司账户扣减) ? true : false;
                    if (!model.ClientId.HasValue)
                    {
                        ret.Code = 500;
                        ret.Msg = "请检查是否存在该客户";
                        break;
                    }
                    //新增消费记录并且扣减账户余额
                    cl_r.CurrentUser = CurrentUser;
                    ChargeLog cl = new ChargeLog
                    {
                        PayType = (isCompany) ? OrderPayType.公司账户扣减 : OrderPayType.账户扣减,
                        ChargeType = ChargeType.消费,
                        Money = p.Money,
                        IsCompany = isCompany
                    };
                    if (model.ClientId.HasValue)
                        cl.ClientId = int.Parse(model.ClientId.ToString());

                    ChargeLog cl_return = cl_r.InsertAndUpdateBalances(cl);

                    if (cl_return == null)
                    {
                        ret.Code = 500;
                        ret.Msg = "扣减金额必须少于或等于账户余额";
                        break;
                    }
                }
            }

            if (ret.Code == 500)
                return ret;

            //更新计划状态为“已完成”
            if (o.SalesPlanId != null)
            {
                var sp = _dbContext.SalesPlans.Where(s => s.Id == o.SalesPlanId).FirstOrDefault();
                sp.State = SalesPlanState.已完成;
            }

            o.LastUpdatedAt = DateTime.Now;

            Save();
            ret.Data = o;

            return ret;
        }
        /// <summary>
        /// 更改订单结算状态
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns></returns>
        public Order ChangePayState(Order model)
        {
            Order o = _dbContext.Orders.Find(model.Id);
            o.PayState = model.PayState;
            o.LastUpdatedAt = DateTime.Now;
            Save();
            return o;
        }
        /// <summary>
        /// 更改订单状态
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns></returns>
        public Order ChangeState(Order order)
        {
            if (order.State == OrderState.空车过磅)
                order.StartOilDateTime = DateTime.Now;
            if (order.State == OrderState.油车过磅)
                order.EndOilDateTime = DateTime.Now;
            //更新对应销售仓的数量
            if (order.State == OrderState.已完成)
            {
                if(order.Unit == "吨")
                    order.OilCount = UnitExchange.ToTon(order.OilCountLitre, order.Density);
                StoreRepository st_r = new StoreRepository(_dbContext);
                //更新油仓数量
                bool isUpdateStore;
                isUpdateStore = st_r.UpdateOil(int.Parse(order.StoreId.ToString()), Math.Round(order.OilCountLitre, 2), false);
                if (isUpdateStore)
                {
                    //增加出仓记录
                    InAndOutLogRepository io_r = new InAndOutLogRepository(_dbContext);
                    io_r.Insert(new InAndOutLog
                    {
                        Name = Enum.GetName(typeof(SalesPlanType), order.OrderType),
                        StoreId = int.Parse(order.StoreId.ToString()),
                        Value = Math.Round(order.OilCount, 2),
                        ValueLitre = Math.Round(order.OilCountLitre, 2),
                        Operators = CurrentUser,
                        Unit = order.Unit,
                        Type = LogType.出仓
                    });
                }
            }
            order.LastUpdatedBy = CurrentUser;
            return Update(order);//更改状态
        }
        /// <summary>
        /// 取得最近的订单model
        /// </summary>
        /// <param name="ordertype">水上|陆上|机油</param>
        /// <returns></returns>
        public Order GetLastOrder(SalesPlanType ordertype)
        {
            return _dbContext.Orders.Where(o => o.OrderType == ordertype).OrderByDescending(o => o.Id).FirstOrDefault();
        }
        /// <summary>
        /// 重新施工
        /// </summary>
        /// <param name="order">model</param>
        /// <returns></returns>
        public Order Restart(Order order)
        {
            StoreRepository st_r = new StoreRepository(_dbContext);
            //更新油仓数量
            bool isUpdateStore = st_r.UpdateOil(int.Parse(order.StoreId.ToString()), order.OilCountLitre , true);
            if (isUpdateStore)
            {
                //增加入仓记录
                InAndOutLogRepository io_r = new InAndOutLogRepository(_dbContext);
                io_r.Insert(new InAndOutLog
                {
                    Name = "重新施工",
                    StoreId = int.Parse(order.StoreId.ToString()),
                    Value = order.OilCount,
                    ValueLitre = order.OilCountLitre,
                    Operators = CurrentUser,
                    Unit = "升",
                    Type = LogType.入仓
                });
            }
            order.State = OrderState.已开单;
            return Update(order);//回退到初始状态“已开单”
        }
        public decimal GetSumNoPay(int cid)
        {
            return _dbContext.Orders.Where(o => o.ClientId == cid && o.PayState == PayState.挂账).Sum(o => o.TotalMoney);
        }
        public decimal GetSumNoPayByCoId(int coId)
        {
            decimal sum = 0;
            int[] cids = _dbContext.Clients.Where(c => c.CompanyId == coId).Select(c => c.Id).ToArray();
            sum = _dbContext.Orders.Where(o => cids.Contains(o.ClientId.Value)).Sum(o => o.TotalMoney);
            return sum;
        }
    }
}
