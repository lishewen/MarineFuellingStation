using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class MoveStoreRepository : RepositoryBase<MoveStore>
    {
        public MoveStoreRepository(EFContext dbContext) : base(dbContext) { }
    }
}
