using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    public class Company : EntityBase
    {
        public TicketType TicketType { get; set; }
        /// <summary>
        /// 发票抬头
        /// </summary>
        public string InvoiceTitle { get; set; }
        /// <summary>
        /// 税号
        /// </summary>
        public string TaxFileNumber { get; set; }
        /// <summary>
        /// 对公账户
        /// </summary>
        public string BusinessAccount { get; set; }
        /// <summary>
        /// 开户银行
        /// </summary>
        public string Bank { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal Balances { get; set; }
        /// <summary>
        /// 总消费金额
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
    public enum TicketType
    {
        普通票,
        专用票
    }
}
