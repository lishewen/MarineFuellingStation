using MFS.Models;
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
        /// <param name="cid"></param>
        /// <returns></returns>
        public ChargeLog InsertAndUpdateClient(ChargeLog model, int cid)
        {
            try { 
                Client client = _dbContext.Clients.Find(cid);
                if (model.ChargeType == ChargeType.充值)
                    client.Balances += model.Money;
                else
                    client.Balances -= model.Money;
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
