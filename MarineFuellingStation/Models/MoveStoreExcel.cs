using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    public class MoveStoreExcel
    {
        public string 单号 { get; set; }
        public string 状态 { get; set; }
        public string 生产员 { get; set; }
        public string 转出仓 { get; set; }
        public decimal 转出油温 { get; set; }
        public double 转出密度 { get; set; }
        public decimal 计划转出升数 { get; set; }
        public decimal 实际转出升数 { get; set; }
        public string 转入仓 { get; set; }
        public decimal 转入油温 { get; set; }
        public double 转入密度 { get; set; }
        public DateTime 创建时间 { get; set; }
    }
}
