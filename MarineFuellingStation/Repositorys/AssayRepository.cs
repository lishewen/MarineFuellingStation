using MFS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class AssayRepository : RepositoryBase<Assay>
    {
        const string tag = "HY";
        public AssayRepository(EFContext dbContext) : base(dbContext) { }
        public string GetLastAssayNo()
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
        /// 获取包含store和purchase对象的集合
        /// </summary>
        /// <returns></returns>
        public List<Assay> GetWithInclude()
        {
            return _dbContext.Assays.Include(a => a.Store).Include(a => a.Purchase).ToList();
        }
        public Assay GetWithInclude(int id)
        {
            return _dbContext.Assays.Where(a => a.Id == id)
                .Include(a => a.Purchase)
                .Include(a => a.Store)
                .FirstOrDefault();
        }
        /// <summary>
        /// 搜索关键字获取包含store和purchase对象的集合
        /// </summary>
        /// <returns></returns>
        public List<Assay> GetWithInclude(string sv)
        {
            return _dbContext.Assays.Where(a => a.Name.Contains(sv)).Include(a => a.Store).Include(a => a.Purchase).ToList();
        }
        /// <summary>
        /// 取得指定时间内记录
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public List<Assay> GetAssaysForExportExcel(DateTime start, DateTime end)
        {
            return _dbContext.Assays.Where(c =>
                c.CreatedAt >= start
                && c.CreatedAt <= end
                && !c.IsDel
                ).Include("Store").Include("Purchase").ToList();
        }
    }
}
