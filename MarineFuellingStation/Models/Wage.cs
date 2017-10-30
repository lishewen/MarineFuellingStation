using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    public class Wage : EntityBase
    {
        public int 年月 { get; set; }
        public string 职务 { get; set; }
        public long DepartmentId { get; set; }
        public decimal 基本 { get; set; }
        public decimal 出勤天数 { get; set; }
        public decimal 绩效工资 { get; set; }
        public decimal 提成 { get; set; }
        public decimal 超额 { get; set; }
        public decimal 交通 { get; set; }
        public decimal 应付 { get; set; }
        public decimal 社保 { get; set; }
        public decimal 请假 { get; set; }
        public decimal 餐费 { get; set; }
        public decimal 借支 { get; set; }
        public decimal 安全保障金 { get; set; }
        public decimal 实发 { get; set; }
        public decimal 转卡金额 { get; set; }
        public decimal 现金 { get; set; }
    }
}
