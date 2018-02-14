using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    public class SurveyExcel
    {
        public string 油仓名称 { get; set; }
        public decimal 油温 { get; set; }
        public double 密度 { get; set; }
        public decimal 油高 { get; set; }
        public decimal 油高对应升数 { get; set; }

    }
}
