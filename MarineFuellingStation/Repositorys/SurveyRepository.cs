using MFS.Models;
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
            return LoadPageList(1, 15, out int rowcount, true, s => s.StoreId == stid, o => o.CreatedAt).ToList();
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

    }
}
