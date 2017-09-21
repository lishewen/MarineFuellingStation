using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    public class User : EntityBase
    {
        public string UserId { get; set; }
        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime ReportDutyTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 基本工资
        /// </summary>
        public decimal BaseWage { get; set; }
        /// <summary>
        /// 社保自负
        /// </summary>
        public decimal SocialSecurity { get; set; }
        /// <summary>
        /// 安全保障金
        /// </summary>
        public decimal Security { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 开户银行
        /// </summary>
        public string Bank { get; set; }
        /// <summary>
        /// 银行账户
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 开户人
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 离职时间
        /// </summary>
        public DateTime? LeaveTime { get; set; }
        /// <summary>
        /// 是否离职
        /// </summary>
        public bool IsLeave { get; set; } = false;
    }
}
