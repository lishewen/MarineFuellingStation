using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    public class StoreExcel
    {
        public string 油仓名称 { get; set; }
        public string 所属 { get; set; }
        public decimal 容量 { get; set; }
        public decimal 数量 { get; set; }
        public double 最近测量密度 { get; set; }
        public DateTime 最近测量时间 { get; set; }
        public string 类型 { get; set; }
    }
}
