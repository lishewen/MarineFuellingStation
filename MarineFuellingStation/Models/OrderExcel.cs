using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    public class OrderExcel
    {
        public string 单号 { get; set; }
        public string 陆上或水上 { get; set; }
        public string 船号或车号 { get; set; }
        public string 计划单号 { get; set; }
        public string 客户 { get; set; }
        public string 销售员 { get; set; }
        public string 商品 { get; set; }
        public decimal 当时最低单价 { get; set; }
        public decimal 单价 { get; set; }
        public decimal 数量 { get; set; }
        public decimal 金额 { get; set; }
        public string 是否开票 { get; set; }
        public decimal 运费 { get; set; }
        public string 开票公司 { get; set; }
        public string 开票类型 { get; set; }
        public decimal 开票单价 { get; set; }
        public decimal 开票数量 { get; set; }
        public string 销售仓 { get; set; }
        public decimal 实际加油数量 { get; set; }
        public string 生产员 { get; set; }
        public double 密度 { get; set; }
        public decimal 油温 { get; set; }
        public decimal 毛重 { get; set; }
        public decimal 皮重 { get; set; }
        public decimal 销售超额提成 { get; set; }

        public string 支付状态 { get; set; }
        public string 收银员 { get; set; }
        public string 备注 { get; set; }
        public string 创建时间 { get; set; }

    }
}
