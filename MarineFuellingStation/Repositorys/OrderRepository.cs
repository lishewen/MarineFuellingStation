using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MFS.Repositorys
{
    public class OrderRepository : RepositoryBase<Order>
    {
        const string tag = "XS";
        public OrderRepository(EFContext dbContext) : base(dbContext) { }

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
            var p = _dbContext.Products.Find(entity.ProductId);
            p.LastPrice = entity.Price;
            entity.MinPrice = p.MinPrice;
            return base.Insert(entity, autoSave);
        }
        /// <summary>
        /// 获取所有订单列表
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
        public List<Order> GetIncludeProduct(int startPage, int pageSize)
        {
            return LoadPageList(startPage, pageSize, out int count, true).Include(o => o.Product).Include(o => o.Client).ToList();
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
        /// 根据PayState获取分页数据，并且包含Product、Client对象
        /// </summary>
        /// <param name="payState">付款状态</param>
        /// <param name="startPage">第N页</param>
        /// <param name="pageSize">每页记录</param>
        /// <returns></returns>
        public List<Order> GetByPayState(PayState payState, int startPage, int pageSize)
        {
            return LoadPageList(startPage, pageSize, out int count, true, o=>o.PayState == payState).Include(o => o.Product).Include(o => o.Client).ToList();
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
            //计算订单销售提成
            if (model.PayState == PayState.已结算)
            {
                o.SalesCommission = (o.Price - o.MinPrice) * o.Count * 0.2M;
            }
            //新增付款记录Payment
            foreach (Payment p in model.Payments)
            {
                //要加载order下才会关联OrderId，直接加在db下不会反应关联关系
                o.Payments.Add(p);
                if (p.PayTypeId == OrderPayType.账户扣减)
                {
                    //扣减账户余额
                    var client = _dbContext.Clients.FirstOrDefault(c => c.CarNo == model.CarNo);
                    if (client != null)
                    {
                        if (client.Balances >= p.Money)
                            client.Balances -= p.Money;
                        else
                        {
                            ret.Code = 500;
                            ret.Msg = "扣减金额必须少于或等于账户余额";
                            break;
                        }
                    }
                }
            }

            //更新计划状态为“已完成”
            if(o.SalesPlanId != null)
            {
                var sp = _dbContext.SalesPlans.Where(s => s.Id == o.SalesPlanId).FirstOrDefault();
                sp.State = SalesPlanState.已完成;
            }

            o.LastUpdatedAt = DateTime.Now;

            if (ret.Code == 0)
            {
                Save();
                ret.Data = o;
            }
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
        public Order ChangeState(Order modelWithChanges)
        {
            //更新对应销售仓的数量
            if(modelWithChanges.State == OrderState.已完成)
            {
                StoreRepository st_r = new StoreRepository(_dbContext);
                //更新油仓数量
                bool isUpdateStore = st_r.UpdateOil(int.Parse(modelWithChanges.StoreId.ToString()), modelWithChanges.Count, false);
                if (isUpdateStore)
                {
                    //增加出仓记录
                    InAndOutLogRepository io_r = new InAndOutLogRepository(_dbContext);
                    io_r.Insert(new InAndOutLog
                    {
                        Name = "订单加油",
                        StoreId = int.Parse(modelWithChanges.StoreId.ToString()),
                        Value = modelWithChanges.Count,
                        Operators = CurrentUser,
                        Unit = "升",
                        Type = LogType.出仓
                    });
                }   
            }
            return Update(modelWithChanges);//更改状态
        }
    }
}
