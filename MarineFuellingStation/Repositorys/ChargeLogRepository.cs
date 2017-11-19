using MFS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class ChargeLogRepository : RepositoryBase<ChargeLog>
    {
        public ChargeLogRepository(EFContext dbContext) : base(dbContext) { }
        /// <summary>
        /// 新增充值或消费记录，并且更新客户或公司余额
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ChargeLog InsertAndUpdateBalances(ChargeLog model)
        {
            try { 
                //公司账户的充值或消费处理
                if (model.IsCompany)
                {
                    Company company = _dbContext.Companys.FirstOrDefault(c => c.Id == model.CompanyId);
                    model.Name = "公司账户";
                    if (model.ChargeType == ChargeType.充值)
                        company.Balances += model.Money;
                    else//消费
                    {
                        if (company.Balances < model.Money)
                            throw new Exception();
                        company.Balances -= model.Money;
                    }
                        
                    CompanyRepository co_r = new CompanyRepository(_dbContext);
                    co_r.Update(company);
                    model.Company = company;
                }
                //客户账户的充值或消费处理
                else
                {
                    Client client = _dbContext.Clients.FirstOrDefault(c => c.Id == model.ClientId);
                    model.Name = "个人账户";
                    if (model.ChargeType == ChargeType.充值)
                        client.Balances += model.Money;
                    else//消费
                    {
                        if (client.Balances < model.Money)
                            throw new Exception();
                        client.Balances -= model.Money;
                    }

                    ClientRepository cl_r = new ClientRepository(_dbContext);
                    cl_r.Update(client);
                    model.Client = client;
                }
                Save();
            }
            catch
            {
                return null;
            }
            return Insert(model);
        }
    }
}
