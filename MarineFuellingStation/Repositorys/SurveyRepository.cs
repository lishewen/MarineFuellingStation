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
            return _dbContext.Surveys.Where(s => s.StoreId == stid).OrderByDescending(o => o.CreatedAt).Take(10).ToList();
        }
        public Survey InsertAndUpdatestore(Survey model)
        {
            var st = _dbContext.Stores.Find(model.StoreId);
            st.LastSurveyAt = DateTime.Now;
            Save();
            return Insert(model);
        }
    }
}
