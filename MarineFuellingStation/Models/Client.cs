using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    public class Client : EntityBase
    {
        public PlaceType PlaceType { get; set; }
        public ClientType ClientType { get; set; }
        public int? CompanyId { get; set; }
        /// <summary>
        /// 跟进销售
        /// </summary>
        public string FollowSalesman { get; set; }
        public string CarNo { get; set; }
        public int? DefaultProductId { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }
        public string Mobile { get; set; }
        public string IdCard { get; set; }
        public string Address { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 最高挂账金额
        /// </summary>
        public decimal MaxOnAccount { get; set; }
        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal Balances { get; set; }
        /// <summary>
        /// 总消费金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        [ForeignKey("DefaultProductId")]
        public virtual Product Product { get; set; }
    }
    public enum PlaceType
    {
        陆上,
        水上
    }
    public enum ClientType
    {
        个人,
        公司,
        全部
    }
}
