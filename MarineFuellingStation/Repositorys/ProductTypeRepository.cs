using MFS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class ProductTypeRepository : RepositoryBase<ProductType>
    {
        public ProductTypeRepository(EFContext dbContext) : base(dbContext) { }
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        public new List<ProductType> GetAllList()
        {
            return _dbContext.ProductTypes.Include(pt => pt.Products).ToList();
        }
    }
}
