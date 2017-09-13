using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    /// <summary>
    /// 测量记录
    /// </summary>
    public class Survey : EntityBase
    {
        /// <summary>
        /// 油温
        /// </summary>
        public decimal Temperature { get; set; }
        /// <summary>
        /// 密度
        /// </summary>
        public decimal Density { get; set; }
        /// <summary>
        /// 油高
        /// </summary>
        public decimal Height { get; set; }
    }
}
