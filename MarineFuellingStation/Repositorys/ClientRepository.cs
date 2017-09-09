using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class ClientRepository : RepositoryBase<Client>
    {
        public ClientRepository(EFContext dbContext) : base(dbContext) { }
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public new Client Insert(Client entity, bool autoSave = true)
        {
            var p = _dbContext.Clients.Find(entity.Id);
            return base.Insert(entity, autoSave);
        }
    }
}
