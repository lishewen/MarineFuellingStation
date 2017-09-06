using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class PurchaseRepository : RepositoryBase<Purchase>
    {
        const string tag = "CG";
        public PurchaseRepository(EFContext dbContext) : base(dbContext) { }
        public string GetLastPurchaseNo()
        {
            try
            {
                return GetAllList().OrderByDescending(o => o.Id).First().Name;
            }
            catch
            {
                return string.Empty;
            }
        }
        public string GetSerialNumber(string serialNumber = "0")
        {
            if (!string.IsNullOrWhiteSpace(serialNumber) && serialNumber != "0")
            {
                if (serialNumber.Length == 10)
                {
                    string headDate = serialNumber.Substring(2, 4);
                    int lastNumber = int.Parse(serialNumber.Substring(6));
                    //如果数据库最大值流水号中日期和生成日期在同一天，则顺序号加1
                    if (headDate == DateTime.Now.ToString("yyMM"))
                    {
                        lastNumber++;
                        return tag + headDate + lastNumber.ToString("0000");
                    }
                }
            }
            return tag + DateTime.Now.ToString("yyMM") + "0001";
        }
    }
}
