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
        /// 新增充值或消费记录，并且更新客户余额
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ChargeLog InsertAndUpdateClient(ChargeLog model)
        {
            try { 
                Client client = _dbContext.Clients.Include("Company").FirstOrDefault(c => c.Id == model.ClientId);
                model.Name = client.CarNo;
                model.CompanyName = client.Company.Name;
                if (model.ChargeType == ChargeType.充值)
                    client.Balances += model.Money;
                else {
                    if (client.Balances < model.Money)
                        throw new Exception();
                    client.Balances -= model.Money;
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
