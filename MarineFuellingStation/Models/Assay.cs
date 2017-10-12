using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    /// <summary>
    /// 化验单
    /// </summary>
    public class Assay : EntityBase
    {
        public AssayType AssayType { get; set; }
        public int? StoreId { get; set; }
        /// <summary>
        /// 化验油仓
        /// </summary>
        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }
        public int? PurchaseId { get; set; }
        /// <summary>
        /// 进油来源
        /// </summary>
        [ForeignKey("PurchaseId")]
        public virtual Purchase Purchase { get; set; }
        public decimal 视密 { get; set; }
        public decimal 标密 { get; set; }
        public string 闭口闪点 { get; set; }
        /// <summary>
        /// 油温
        /// </summary>
        public decimal Temperature { get; set; }
        /// <summary>
        /// 味道
        /// </summary>
        public SmellType SmellType { get; set; }
        public string 混水反应 { get; set; }
        public string 十六烷值 { get; set; }
        public decimal 初硫 { get; set; }
        /// <summary>
        /// 10%
        /// </summary>
        public decimal Percentage10 { get; set; }
        /// <summary>
        /// 50%
        /// </summary>
        public decimal Percentage50 { get; set; }
        /// <summary>
        /// 90%
        /// </summary>
        public decimal Percentage90 { get; set; }
        public decimal 回流 { get; set; }
        public decimal 干点 { get; set; }
        /// <summary>
        /// 化验员
        /// </summary>
        public string Assayer { get; set; }
        public bool IsUse { get; set; }
    }

    public enum AssayType
    {
        油舱化验,
        进油化验
    }
    public enum SmellType
    {
        一般刺鼻,
        刺鼻,
        不刺鼻
    }
}
