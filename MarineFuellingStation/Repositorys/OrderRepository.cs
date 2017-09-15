using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MFS.Repositorys
{
    public class OrderRepository : RepositoryBase<Order>
    {
        const string tag = "XS";
        public OrderRepository(EFContext dbContext) : base(dbContext) { }

        public string GetLastOrderNo()
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
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public new Order Insert(Order entity, bool autoSave = true)
        {
            var p = _dbContext.Products.Find(entity.ProductId);
            p.LastPrice = entity.Price;
            return base.Insert(entity, autoSave);
        }
        public List<Order> GetIncludeProduct(int startPage, int pageSize)
        {
            return LoadPageList(startPage, pageSize, out int count, true).Include(o => o.Product).ToList();
        }
    }
}
