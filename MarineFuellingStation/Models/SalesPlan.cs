using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    /// <summary>
    /// 销售计划 Name字段为单号
    /// </summary>
    public class SalesPlan : EntityBase
    {
        public SalesPlanType SalesPlanType { get; set; }
        public string CarNo { get; set; }
        public int ProductId { get; set; }
        /// <summary>
        /// 油品名
        /// </summary>
        public string OilName { get; set; }
        public decimal Price { get; set; }
        public decimal Count { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 当前余油
        /// </summary>
        public decimal Remainder { get; set; }
        public DateTime OilDate { get; set; }
        /// <summary>
        /// 是否开票
        /// </summary>
        public bool IsInvoice { get; set; }
        /// <summary>
        /// 送货上门/自提
        /// </summary>
        public bool IsDeliver { get; set; } = true;
        /// <summary>
        /// 送货上门 运费
        /// </summary>
        public decimal DeliverMoney { get; set; } = 0;
        /// <summary>
        /// 是否打印单价
        /// </summary>
        public bool IsPrintPrice { get; set; } = true;
        public TicketType TicketType { get; set; }
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
        public decimal BillingCount { get; set; }
        public SalesPlanState State { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string Auditor { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal TotalMoney
        {
            get
            {
                return Math.Round(Price * Count, 2);
            }
        }
        /// <summary>
         /// 备注
         /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 单据是否删除标识
        /// </summary>
        public bool IsDel { get; set; } = false;
    }

    public enum SalesPlanType
    {
        水上加油,
        陆上装车,
        水上机油,
        全部,
        陆上公司车,
        陆上外来车
    }

    public enum SalesPlanState
    {
        未审批,
        已审批,
        已完成
    }
}
