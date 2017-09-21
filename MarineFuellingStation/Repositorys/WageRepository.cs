using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class WageRepository : RepositoryBase<Wage>
    {
        public WageRepository(EFContext dbContext) : base(dbContext) { }
    }
}
