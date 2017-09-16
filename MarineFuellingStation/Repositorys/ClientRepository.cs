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
        public List<Client> GetIncludeCompany()
        {
            return _dbContext.Clients.Include("Company").ToList();
        }
        public List<Client> GetMyClients(ClientType ctype)
        {
            if(ctype == ClientType.全部)
                return _dbContext.Clients.Include("Company").Where(c => c.FollowSalesman == CurrentUser && (c.ClientType == ClientType.个人 || c.ClientType == ClientType.公司)).ToList();
            else
                return _dbContext.Clients.Include("Company").Where(c => c.FollowSalesman == CurrentUser && c.ClientType == ctype).ToList();
        }
    }
}
