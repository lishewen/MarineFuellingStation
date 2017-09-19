using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    /// <summary>
    /// name 为order的单号
    /// </summary>
    public class Payment: EntityBase
    {
        /// <summary>
        /// 支付方式
        /// </summary>
        public OrderPayType PayTypeId { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        public int? OrderId { get; set; }
        [JsonIgnore,ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}
