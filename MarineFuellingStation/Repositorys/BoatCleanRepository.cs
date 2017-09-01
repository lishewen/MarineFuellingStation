using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class BoatCleanRepository : RepositoryBase<BoatClean>
    {
        public BoatCleanRepository(EFContext dbContext) : base(dbContext) { }
    }
}
