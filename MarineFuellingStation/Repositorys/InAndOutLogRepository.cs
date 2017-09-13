using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class InAndOutLogRepository : RepositoryBase<InAndOutLog>
    {
        public InAndOutLogRepository(EFContext dbContext) : base(dbContext) { }
    }
}
