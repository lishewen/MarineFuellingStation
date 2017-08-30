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
    }
}
