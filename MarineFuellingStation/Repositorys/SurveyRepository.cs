using MFS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class SurveyRepository : RepositoryBase<Survey>
    {
        public SurveyRepository(EFContext dbContext) : base(dbContext) { }
        public List<Survey> Top15(int stid)
        {
            return LoadPageList(1, 15, out int rowcount, true, false, s => s.StoreId == stid, o => o.CreatedAt).ToList();
        }
        /// <summary>
        /// 新增并更新油仓当前密度
        /// </summary>
        /// <param name="model"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        public new Survey Insert(Survey model, bool autoSave = true)
        {
            var st = _dbContext.Stores.Find(model.StoreId);
            st.Density = model.Density;
            st.LastSurveyAt = DateTime.Now;
            return base.Insert(model, autoSave);
        }
        /// <summary>
        /// 取得指定时间内记录
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public List<Survey> GetSurveysForExportExcel(DateTime start, DateTime end)
        {
            return _dbContext.Surveys.Where(c =>
                c.CreatedAt >= start
                && c.CreatedAt <= end
                && !c.IsDel
                ).Include("Store").ToList();
        }
    }
}
