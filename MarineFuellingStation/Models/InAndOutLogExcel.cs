using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    public class InAndOutLogExcel
    {
        public string 操作 { get; set; }
        public string 出仓入仓 { get; set; }
        public string 油仓 { get; set; }
        public decimal 数量 { get; set; }
        public string 单位 { get; set; }
        public decimal 数量升数 { get; set; }
        public string 操作人员 { get; set; }
        public string 时间 { get; set; }
    }
}
