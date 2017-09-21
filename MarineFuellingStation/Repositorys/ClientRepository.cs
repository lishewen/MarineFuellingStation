using MFS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace MFS.Repositorys
{
    public class ClientRepository : RepositoryBase<Client>
    {
        public ClientRepository(EFContext dbContext) : base(dbContext) { }
        public List<Client> GetIncludeCompany()
        {
            return _dbContext.Clients.Include("Company").ToList();
        }
        public Client GetDetail(int id)
        {
            return _dbContext.Clients.Include("Company").Include("Product").FirstOrDefault(c => c.Id == id);
        }
        public List<Client> GetMyClients(ClientType ctype, int ptype, int balances, int cycle)
        {
            List<Client> list;

            Expression<Func<Client, bool>> clientwhere = c => c.FollowSalesman == CurrentUser;

            if (ptype > 0)//计划状态
            {
                var sts = (SalesPlanState)ptype;
                var clist = _dbContext.SalesPlans.Where(s => s.State == sts).Select(s => s.CarNo);
                clientwhere = clientwhere.And(c => clist.Contains(c.CarNo));
            }

            if (balances > 0)//余额
                clientwhere.And(c => c.Balances < balances);

            if (cycle > 0)//周期
            {
                var cylist = _dbContext.SalesPlans.Where(s => (s.LastUpdatedAt - DateTime.Now).Days > cycle).Select(s => s.CarNo);
                clientwhere = clientwhere.And(c => cylist.Contains(c.CarNo));
            }

            if (ctype == ClientType.全部)
                clientwhere.And(c => (c.ClientType == ClientType.个人 || c.ClientType == ClientType.公司));
            else
                clientwhere.And(c => c.ClientType == ctype);

            list = _dbContext.Clients.Include("Company").Where(clientwhere).ToList();

            return list;
        }
    }
}
