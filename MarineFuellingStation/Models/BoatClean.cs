using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    /// <summary>
    /// 船舶清污单
    /// </summary>
    public class BoatClean : EntityBase
    {
        public BoatClean()
        {
            if (Payments == null)
                Payments = new List<Payment>();
        }
        /// <summary>
        /// 船号
        /// </summary>
        public string CarNo { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 航次
        /// </summary>
        public int Voyage { get; set; }
        /// <summary>
        /// 吨位
        /// </summary>
        public decimal Tonnage { get; set; }
        /// <summary>
        /// 批文号
        /// </summary>
        public string ResponseId { get; set; }
        /// <summary>
        /// 作业地点
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 作业单位
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
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
        /// 开始作业时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public BoatCleanPayState PayState { get; set; }
        /// <summary>
        /// 支付金额和方式
        /// </summary>
        public virtual ICollection<Payment> Payments { get; set; }
        public BoatCleanState State { get; set; } = BoatCleanState.已开单;
    }

    public enum BoatCleanState
    {
        已开单,
        施工中,
        已完成
    }
    public enum BoatCleanPayState
    {
        未结算,
        已结算,
        挂账
    }
}
