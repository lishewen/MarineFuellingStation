using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    /// <summary>
    /// 销售单 Name字段为单号
    /// </summary>
    public class Order : EntityBase
    {
        public int? SalesPlanId { get; set; }
        [ForeignKey("SalesPlanId")]
        public virtual SalesPlan SalesPlan { get; set; }
        public SalesPlanType OrderType { get; set; }
        /// <summary>
        /// 船号/车号
        /// </summary>
        public string CarNo { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        /// <summary>
        /// 商品单价
        /// </summary>
        public decimal Price { get; set; }
        public int Count { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal TotalMoney
        {
            get
            {
                return Price * Count;
            }
        }
        /// <summary>
        /// 是否开票
        /// </summary>
        public bool IsInvoice { get; set; }
        /// <summary>
        /// 开票单位
        /// </summary>
        public string BillingCompany { get; set; }
        /// <summary>
        /// 开票单价
        /// </summary>
        public decimal BillingPrice { get; set; }
        /// <summary>
        /// 开票数量
        /// </summary>
        public int BillingCount { get; set; }
        /// <summary>
        /// 实际加油数量
        /// </summary>
        public decimal OilCount { get; set; }
        /// <summary>
        /// 生产员 以'|'区分多个
        /// </summary>
        public string Worker { get; set; }
        /// <summary>
        /// 开始装油时间
        /// </summary>
        public DateTime? StartOilDateTime { get; set; }
        /// <summary>
        /// 结束装油时间
        /// </summary>
        public DateTime? EndOilDateTime { get; set; }
        /// <summary>
        /// 表1
        /// </summary>
        public decimal Instrument1 { get; set; }
        /// <summary>
        /// 表2
        /// </summary>
        public decimal Instrument2 { get; set; }
        /// <summary>
        /// 表3
        /// </summary>
        public decimal Instrument3 { get; set; }
        /// <summary>
        /// 密度
        /// </summary>
        public decimal Density { get; set; }
        /// <summary>
        /// 油温
        /// </summary>
        public decimal OilTemperature { get; set; }
        /// <summary>
        /// 实际与订单差
        /// </summary>
        public decimal DiffOil { get; set; }
        /// <summary>
        /// 皮重 陆上（空车）
        /// </summary>
        public decimal EmptyCarWeight { get; set; }
        /// <summary>
        /// 毛重 陆上(油 + 车)
        /// </summary>
        public decimal OilCarWeight { get; set; }
        /// <summary>
        /// 油重 陆上
        /// </summary>
        public decimal DiffWeight { get; set; }
        /// <summary>
        /// 销售提成
        /// </summary>
        public decimal SalesCommission { get; set; }
        public int? TransportOrderId { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderState State { get; set; } = OrderState.已开单;
        public TicketType TicketType { get; set; }
        /// <summary>
        /// 是否运输
        /// </summary>
        public bool IsTrans { get; set; }
        /// <summary>
        /// 油车磅秤图片地址 陆上加油
        /// </summary>
        public string OilCarWeightPic { get; set; }
        /// <summary>
        /// 空车车磅秤图片地址 陆上加油
        /// </summary>
        public string EmptyCarWeightPic { get; set; }
        /// <summary>
        /// 销售仓Id 陆上加油
        /// </summary>
        public int? StoreId { get; set; }
        /// <summary>
        /// 对应油仓
        /// </summary>
        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }

    }
    public enum OrderState
    {
        已开单,
        选择销售仓,
        空车过磅,
        装油中,
        油车过磅,
        已完成,
        已结算,
        挂账
    }
    public enum OrderPayType
    {
        现金,
        微信,
        支付宝,
        刷卡一,
        刷卡二,
        刷卡三
    }
}
