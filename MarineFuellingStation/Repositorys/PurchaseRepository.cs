using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class PurchaseRepository : RepositoryBase<Purchase>
    {
        public PurchaseRepository(EFContext dbContext) : base(dbContext) { }
    }
}
