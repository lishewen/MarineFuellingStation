using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class StoreRepository : RepositoryBase<Store>
    {
        public StoreRepository(EFContext dbContext) : base(dbContext) { }
        /// <summary>
        /// 生产完毕后更新油仓数量
        /// </summary>
        /// <param name="inStId">入仓油仓</param>
        /// <param name="outStId">出仓油仓</param>
        /// <param name="inCount">进油数量</param>
        /// <param name="outCount">出油数量</param>
        /// <returns></returns>
        public bool UpdateOil(int inStId, int outStId, decimal inCount, decimal outCount)
        {
            try
            {
                var st_in = _dbContext.Stores.Find(inStId);
                var st_out = _dbContext.Stores.Find(outStId);
                st_in.Value += inCount;
                st_out.Value -= outCount;
                Save();
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 加油或卸油完毕后更新油仓数量
        /// </summary>
        /// <param name="outStId">出仓油仓</param>
        /// <param name="outCount">出油数量</param>
        /// <returns></returns>
        public bool UpdateOil(int storeId, decimal count, bool isInStore)
        {
            try
            {
                var st = _dbContext.Stores.Find(storeId);
                st.Value = isInStore? st.Value + count : st.Value - count;
                Save();
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 更新平均单价
        /// </summary>
        /// <param name="storeId">卸油油仓Id</param>
        /// <param name="purchasePrice">采购单价 单位：元/升</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public bool UpdateAvgPrice(int storeId, decimal purchasePrice, decimal count)
        {
            try
            {
                var st = _dbContext.Stores.Find(storeId);
                st.AvgPrice = (st.AvgPrice * st.Value + purchasePrice * count) / (st.Value + count);
                Save();
            }
            catch
            {
                return false;
            }
            return true;
        }
        public List<Store> GetByClass(StoreClass sc)
        {
            if (sc == StoreClass.全部)
                return LoadPageList(1,30,out int count,true,s => s.StoreClass == StoreClass.存储仓 || s.StoreClass == StoreClass.销售仓).OrderBy(s => s.Name).ToList();
            else
                return LoadPageList(1, 30, out int count, true, s => s.StoreClass == sc).OrderBy(s => s.Name).ToList();
        }
    }
}
