using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public abstract class EntityBase : ITrackable
    {
        /// <summary>
        /// 主键Id (主键类型根据继承时确定)
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "名称")]
        [Required(ErrorMessage = "不能为空")]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }
        /// <summary>
        /// 单据是否删除标识
        /// </summary>
        public bool IsDel { get; set; } = false;
        /// <summary>
        /// 删单原因
        /// </summary>
        public string DelReason { get; set; }
    }
}
