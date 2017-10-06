using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    /// <summary>
    /// 通知
    /// </summary>
    public class Notice : EntityBase
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUse { get; set; } = true;
        /// <summary>
        /// 所通知的应用，多应用用'|'分开
        /// </summary>
        public string ToApps { get; set; }
        public enum Apps {
            销售计划,
            销售单,
            陆上卸油,
            陆上装油,
            水上加油
        }

    }
}
