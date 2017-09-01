using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class AssayRepository : RepositoryBase<Assay>
    {
        public AssayRepository(EFContext dbContext) : base(dbContext) { }
    }
}
