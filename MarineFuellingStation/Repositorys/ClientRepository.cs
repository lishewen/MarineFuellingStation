using MFS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class ClientRepository : RepositoryBase<Client>
    {
        public ClientRepository(EFContext dbContext) : base(dbContext) { }
        public new List<Client> GetIncludeCompany()
        {
            return _dbContext.Clients.Include("Company").ToList();
        }
    }
}
