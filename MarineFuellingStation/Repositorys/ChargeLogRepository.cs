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
        public ChargeLog InsertAndUpdateBalances(ChargeLog model, bool isCompanyCharge)
        {
            try { 
                Client client = _dbContext.Clients.Include("Company").FirstOrDefault(c => c.Id == model.ClientId);
                model.CompanyName = client.Company.Name;
                //公司账户的充值或消费处理
                if (isCompanyCharge)
                {
                    model.Name = "公司账户";
                    if (model.ChargeType == ChargeType.充值)
                        client.Company.Balances += model.Money;
                    else
                    {
                        if (client.Company.Balances < model.Money)
                            throw new Exception();
                        client.Company.Balances -= model.Money;
                    }
                        
                    CompanyRepository co_r = new CompanyRepository(_dbContext);
                    co_r.Update(client.Company);
                }
                //客户账户的充值或消费处理
                else
                {
                    model.Name = "个人账户";
                    if (model.ChargeType == ChargeType.充值)
                        client.Balances += model.Money;
                    else
                    {
                        if (client.Balances < model.Money)
                            throw new Exception();
                        client.Balances -= model.Money;
                    }
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
