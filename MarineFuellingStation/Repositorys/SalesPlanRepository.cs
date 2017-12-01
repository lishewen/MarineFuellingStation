using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class SalesPlanRepository : RepositoryBase<SalesPlan>
    {
        const string tag = "JH";
        public SalesPlanRepository(EFContext dbContext) : base(dbContext) { }

        /// <summary>
        /// 获取销售计划编号
        /// </summary>
        public string GetLastSalesPlanNo()
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

        /// <summary>
        /// 销售计划流水号生成方法 JH17070001
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
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
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public new SalesPlan Insert(SalesPlan entity, bool autoSave = true)
        {
            var p = _dbContext.Products.Find(entity.ProductId);
            p.LastPrice = entity.Price;
            if (!entity.IsInvoice)
            {
                entity.BillingCompany = "";
                entity.BillingCount = 0;
                entity.BillingPrice = 0;
            }
            return base.Insert(entity, autoSave);
        }
        public SalesPlan GetDetail(int id)
        {
            return _dbContext.SalesPlans.FirstOrDefault(s => s.Id == id);
        }
    }
}
