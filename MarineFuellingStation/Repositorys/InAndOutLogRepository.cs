using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MFS.Repositorys
{
    public class InAndOutLogRepository : RepositoryBase<InAndOutLog>
    {
        public InAndOutLogRepository(EFContext dbContext) : base(dbContext) { }

        public List<InAndOutLog> GetIncludeStore(LogType type, int page)
        {
            List<InAndOutLog> list;
            if (type == LogType.全部)
                list = LoadPageList(page, 10, out int rowCount, true, false, i => i.Type == LogType.入仓 || i.Type == LogType.出仓).Include(i => i.Store).ToList();
            else
                list = LoadPageList(page, 10, out int rowCount, true, false, i => i.Type == type).Include(i => i.Store).ToList();
            foreach(InAndOutLog io in list)
            {
                Store st = io.Store;
                string stName = _dbContext.StoreTypes.FirstOrDefault(stt => stt.Id == st.StoreTypeId).Name;
                io.Store.StoreTypeName = stName;
            }
            return list;
        }

        public decimal GetStoreSumValue(int id, LogType type, DateTime date)
        {
            DateTime startdate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            DateTime enddate = startdate.AddDays(1).AddSeconds(-1);
            decimal sumValue = GetAllList(i => i.Type == type && i.StoreId == id && i.LastUpdatedAt >= startdate && i.LastUpdatedAt <= enddate).Sum(i => i.ValueLitre);
            return Math.Round(sumValue, 2);
        }
        /// <summary>
        /// 取得指定时间内记录
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public List<InAndOutLog> GetSurveysForExportExcel(DateTime start, DateTime end)
        {
            return _dbContext.InAndOutLogs.Where(c =>
                c.CreatedAt >= start
                && c.CreatedAt <= end
                && !c.IsDel
                ).Include("Store").ToList();
        }
    }
}
