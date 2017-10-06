using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class NoticeRepository : RepositoryBase<Notice>
    {
        public NoticeRepository(EFContext dbContext) : base(dbContext) { }
    }
}
