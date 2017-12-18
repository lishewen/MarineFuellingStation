using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class CompanyRepository : RepositoryBase<Company>
    {
        public CompanyRepository(EFContext dbContext) : base(dbContext) { }
        public List<Company> GetWithClients()
        {
            List<Company> companys = GetAllList();
            if(companys != null)
            {
                foreach (Company co in companys)
                {
                    co.Clients = new List<Client>();
                    List<Client> clients = _dbContext.Clients.Where(c => c.CompanyId == co.Id).ToList();
                    if (clients != null)
                    {
                        foreach(Client c in clients)
                        {
                            co.Clients.Add(new Client
                            {
                                Name = c.Name,
                                CarNo = c.CarNo,
                                Id = c.Id
                            });
                        }
                    }
                }
            }
            return companys;
        }
        
    }
}
