using MFS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class PurchaseRepository : RepositoryBase<Purchase>
    {
        const string tag = "CG";
        public PurchaseRepository(EFContext dbContext) : base(dbContext) { }
        public string GetLastPurchaseNo()
        {
            try
            {
                return GetAllList().OrderByDescending(o => o.Id).First().Name;
            }
            catch
            {
                return string.Empty;
            }
        }
        public string GetSerialNumber(string serialNumber = "0")
        {
            if (!string.IsNullOrWhiteSpace(serialNumber) && serialNumber != "0")
            {
                if (serialNumber.Length == 10)
                {
                    string headDate = serialNumber.Substring(2, 4);
                    int lastNumber = int.Parse(serialNumber.Substring(6));
                    //如果数据库最大值流水号中日期和生成日期在同一天，则顺序号加1
                    if (headDate == DateTime.Now.ToString("yyMM"))
                    {
                        lastNumber++;
                        return tag + headDate + lastNumber.ToString("0000");
                    }
                }
            }
            return tag + DateTime.Now.ToString("yyMM") + "0001";
        }
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        public List<Purchase> GetIncludeProduct()
        {
            return _dbContext.Purchases.Include(p => p.Product).Where(p => !p.IsDel).ToList();
        }
        /// <summary>
        /// 获取实体，并关联所有外键的model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Purchase GetWithInclude(int id)
        {
            return _dbContext.Purchases.Where(p => p.Id == id && !p.IsDel)
                .Include(p => p.Assay)
                .Include(p => p.Product)
                .FirstOrDefault();
        }

        /// <summary>
        /// 获取待卸油施工的采购单
        /// </summary>
        /// <returns></returns>
        public List<Purchase> GetReadyUnload()
        {
            List<Purchase> purchases = _dbContext.Purchases.Where(p => p.State != Purchase.UnloadState.已审核 && !p.IsDel).Include(p => p.Product).OrderByDescending(p => p.Id).ToList();
            foreach(var p in purchases)
            {
                if (!string.IsNullOrEmpty(p.ToStoreIds))
                {
                    p.ToStoresList = GetToStoresList(p);
                }
            }
            return purchases;
        }
        /// <summary>
        /// 获取最近top n个进油单
        /// </summary>
        /// <param name="n">top n?</param>
        /// <returns></returns>
        public List<Purchase> GetTopNPurchases(int n)
        {
            return LoadPageList(1, n, out int rowcount, true).Include(p => p.Product).ToList();
        }
        public Purchase GetDetail(int id)
        {
            return _dbContext.Purchases.Include("Product").FirstOrDefault(p => p.Id == id);
        }
        /// <summary>
        /// 卸油审核后更新油仓油量，平均单价；新增出入记录
        /// </summary>
        /// <param name="model">进油单model</param>
        /// <returns>实际入仓总升数</returns>
        public decimal UpdateStoreOil(Purchase model)
        {
            decimal infactTotal = 0;
            try { 
                //更新油仓
                StoreRepository st_r;
                InAndOutLogRepository io_r;
                st_r = new StoreRepository(_dbContext);
                foreach (ToStoreModel ts in model.ToStoresList)
                {
                    //更新平均单价
                    bool isUpdateAvgPrice = st_r.UpdateAvgPrice(ts.Id, model.Price, ts.Count);
                    //更新油仓当前数量
                    bool isUpdateStore = st_r.UpdateOil(ts.Id, ts.Count, true);
                    if (isUpdateStore && isUpdateAvgPrice)
                    {
                        //增加入仓记录
                        io_r = new InAndOutLogRepository(_dbContext);
                        io_r.Insert(new InAndOutLog
                        {
                            Name = "卸油入库",
                            StoreId = ts.Id,
                            Value = ts.Count,
                            ValueLitre = ts.Count,
                            Operators = CurrentUser,
                            Unit = "升",
                            Type = LogType.入仓
                        });
                        infactTotal += ts.Count;
                    }
                }
                return infactTotal;
            }
            catch
            {
                return 0;
            }
        }
        public List<Purchase> GetByState(int page, int pageSize, Purchase.UnloadState pus)
        {
            List<Purchase> list = LoadPageList(page, pageSize, out int rCount, true, false, p => p.State == pus).OrderByDescending(p => p.Id).ToList();
            //根据ids,names,counts添加入toStoresList
            foreach (var p in list)
            {
                if (!string.IsNullOrEmpty(p.ToStoreIds))
                {
                    p.ToStoresList = GetToStoresList(p);
                }
            }
            return list;
        }
        private List<ToStoreModel> GetToStoresList(Purchase p)
        {
            List<ToStoreModel> ToStoresList = new List<ToStoreModel>();
            //多个
            if (p.ToStoreIds.IndexOf(',') > -1)
            {
                var arrSid = p.ToStoreIds.Split(',').ToArray();
                var arrName = p.ToStoreNames.Split(',').ToArray();
                var arrCount = p.ToStoreCounts.Split(',').ToArray();
                for (int i = 0; i < arrSid.Length; i++)
                {
                    ToStoreModel s = new ToStoreModel();
                    s.Id = int.Parse(arrSid[i]); s.Name = arrName[i]; s.Count = Convert.ToDecimal(arrCount[i]);
                    ToStoresList.Add(s);
                }
            }
            //单个
            else
            {
                ToStoreModel s = new ToStoreModel();
                s.Id = int.Parse(p.ToStoreIds); s.Name = p.ToStoreNames; s.Count = Convert.ToDecimal(p.ToStoreCounts);
                ToStoresList.Add(s);
            }
            return ToStoresList;
        }
    }
}
