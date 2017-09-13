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
    }
}
