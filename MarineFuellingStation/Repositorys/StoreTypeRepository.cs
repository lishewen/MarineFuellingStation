using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MFS.Models;

namespace MFS.Repositorys
{
    public class StoreTypeRepository : RepositoryBase<StoreType>
    {
        public StoreTypeRepository(EFContext dbContext) : base(dbContext) { }
    }
}
