using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    /// <summary>
    /// 出入仓记录
    /// </summary>
    public class InAndOutLog : EntityBase
    {
        public LogType Type { get; set; }
        public int StoreId { get; set; }
        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }
        /// <summary>
        /// 操作员 | 分隔
        /// </summary>
        public string Operators { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 对应单位的值
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// 单位为升的值
        /// </summary>
        public decimal ValueLitre { get; set; }
    }

    public enum LogType
    {
        出仓,
        入仓,
        全部
    }
}
