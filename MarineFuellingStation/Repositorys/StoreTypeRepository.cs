using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MFS.Models;
using Microsoft.EntityFrameworkCore;

namespace MFS.Repositorys
{
    public class StoreTypeRepository : RepositoryBase<StoreType>
    {
        public StoreTypeRepository(EFContext dbContext) : base(dbContext) { }
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        public new List<StoreType> GetAllList()
        {
            return _dbContext.StoreTypes.Include(st => st.Stores).ToList();
        }
    }
}
