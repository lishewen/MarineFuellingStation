using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MFS.Repositorys
{
    public class InAndOutLogRepository : RepositoryBase<InAndOutLog>
    {
        public InAndOutLogRepository(EFContext dbContext) : base(dbContext) { }

        public List<InAndOutLog> GetIncludeStore(LogType type, int page)
        {
            if(type == LogType.全部)
                return LoadPageList(page, 10, out int rowCount, true, i => i.Type == LogType.入仓 || i.Type == LogType.出仓).Include(i => i.Store).ToList();
            else
                return LoadPageList(page, 10, out int rowCount, true, i => i.Type == type).Include(i => i.Store).ToList();
        }
    }
}
