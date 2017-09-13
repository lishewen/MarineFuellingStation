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
        public List<Survey> Top10(int stid)
        {
            return LoadPageList(1, 10, out int rowcount, true, s => s.StoreId == stid, o => o.CreatedAt).ToList();
        }
        public new Survey Insert(Survey model, bool autoSave = true)
        {
            var st = _dbContext.Stores.Find(model.StoreId);
            st.LastSurveyAt = DateTime.Now;
            return base.Insert(model, autoSave);
        }
    }
}
