using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class StoreRepository : RepositoryBase<Store>
    {
        public StoreRepository(EFContext dbContext) : base(dbContext) { }
    }
}
