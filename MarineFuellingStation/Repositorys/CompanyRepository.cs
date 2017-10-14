using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class CompanyRepository : RepositoryBase<Company>
    {
        public CompanyRepository(EFContext dbContext) : base(dbContext) { }
        
    }
}
