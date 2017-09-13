using MFS.Models;
using Microsoft.EntityFrameworkCore;
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
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        public List<Purchase> GetIncludeProduct()
        {
            return _dbContext.Purchases.Include(p => p.Product).ToList();
        }
        /// <summary>
        /// 获取最近top n个采购单
        /// </summary>
        /// <param name="n">top n?</param>
        /// <returns></returns>
        public List<Purchase> GetTopNPurchases(int n)
        {
            return LoadPageList(1, n, out int rowcount, true).ToList();
        }        
    }
}
