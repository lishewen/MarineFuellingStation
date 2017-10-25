using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class ProductRepository : RepositoryBase<Product>
    {
        public ProductRepository(EFContext dbContext) : base(dbContext) { }
        public void Init()
        {
            CurrentUser = "System";
            var p = new ProductType
            {
                Name = "油品"
            };
            p.Products.Add(new Product
            {
                Name = "93#",
                MinPrice = 93
            });
            p.Products.Add(new Product
            {
                Name = "95#",
                MinPrice = 95
            });
            p.Products.Add(new Product
            {
                Name = "97#",
                MinPrice = 97
            });
            _dbContext.ProductTypes.Add(p);
            _dbContext.SaveChanges();
        }
        /// <summary>
        /// 批量修改单价
        /// </summary>
        /// <param name="list"></param>
        /// <returns>涉及更改的数量</returns>
        public int ModifyProdPrice(List<Product> list)
        {
            int count = 0;
            try {
                foreach(Product p in list)
                {
                    var pOld = Get(p.Id);
                    if (pOld.MinPrice != p.MinPrice)
                    {
                        pOld.MinPrice = p.MinPrice;
                        count++;
                    }
                }
                Save();
                return count;
            }
            catch
            {
                return -1;
            }
        }
    }
}
