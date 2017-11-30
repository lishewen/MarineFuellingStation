using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    /// <summary>
    /// 测量记录
    /// </summary>
    public class Survey : EntityBase
    {
        public int StoreId { get; set; }
        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }
        /// <summary>
        /// 油温
        /// </summary>
        public decimal Temperature { get; set; }
        /// <summary>
        /// 密度
        /// </summary>
        public double Density { get; set; }
        /// <summary>
        /// 油高
        /// </summary>
        public decimal Height { get; set; }
        /// <summary>
        /// 油高对应的升数
        /// </summary>
        public decimal Count { get; set; }
    }
}
