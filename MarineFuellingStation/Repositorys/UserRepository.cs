using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(EFContext dbContext) : base(dbContext) { }
    }
}
