using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    public class Company : EntityBase
    {
        /// <summary>
        /// 搜索用的关键字
        /// </summary>
        public string Keyword { get; set; }
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
        public decimal Balances { get; set; } = 0;
        /// <summary>
        /// 总消费金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 客户成员
        /// </summary>
        [NotMapped]
        public List<Client> Clients { get; set; }
    }
    public enum TicketType
    {
        //普通票,
        //专用票,
        循票,
        柴票
    }
}
