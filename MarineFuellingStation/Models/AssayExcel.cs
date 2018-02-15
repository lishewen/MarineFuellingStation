using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    public class AssayExcel
    {
        public string 单号 { get; set; }
        public string 类型 { get; set; }
        public string 油仓 { get; set; }
        public string 卸油单 { get; set; }
        public double 视密 { get; set; }
        public double 标密 { get; set; }
        public string 闭口闪点 { get; set; }
        public decimal 油温 { get; set; }
        public string 量油温时间 { get; set; }
        public string 味道 { get; set; }
        public string 混水反应 { get; set; }
        public string 十六烷值 { get; set; }
        public string 十六烷指数 { get; set; }
        public decimal 初硫 { get; set; }
        public decimal 百分十 { get; set; }
        public decimal 百分五十 { get; set; }
        public decimal 百分九十 { get; set; }
        public decimal 回流 { get; set; }
        public decimal 干点 { get; set; }
        public decimal 蚀点 { get; set; }
        public decimal 凝点 { get; set; }
        public decimal 含硫 { get; set; }
        public string 化验员 { get; set; }
        public string 化验时间 { get; set; }

    }
}
