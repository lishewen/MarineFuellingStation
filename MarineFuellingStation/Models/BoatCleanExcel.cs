using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    public class BoatCleanExcel
    {
        public string 单号 { get; set; }
        public string 船号 { get; set; }
        public decimal 金额 { get; set; }
        public int 航次 { get; set; }
        public decimal 吨位 { get; set; }
        public string 批文号 { get; set; }
        public string 作业地点 { get; set; }
        public string 作业单位 { get; set; }
        public string 联系电话 { get; set; }
        public string 是否开票 { get; set; }
        public string 开票单位 { get; set; }
        public string 开票单价 { get; set; }
        public int 开票数量 { get; set; }
        public string 支付状态 { get; set; }
        public string 支付金额和方式 { get; set; }
        public string 施工人员 { get; set; }
    }
}
