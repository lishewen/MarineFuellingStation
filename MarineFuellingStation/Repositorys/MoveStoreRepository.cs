using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class MoveStoreRepository : RepositoryBase<MoveStore>
    {
        const string tag = "ZC";
        public MoveStoreRepository(EFContext dbContext) : base(dbContext) { }
        public string GetLastMoveStoreNo()
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
        public List<Models.GET.MoveStore> GetForIsFinished(bool isFinished)
        {
            List<MoveStore> list;
            if(isFinished)
                list = GetAllList(m => m.State == MoveStoreState.已完成);
            else
                list = GetAllList(m => m.State == MoveStoreState.已开单 || m.State == MoveStoreState.施工中);
            List<Models.GET.MoveStore> newlist = new List<Models.GET.MoveStore>();
            foreach (MoveStore m in list)
            {
                string stateName = "";
                switch (m.State)
                {
                    case MoveStoreState.已开单:
                        stateName = "已开单";
                        break;
                    case MoveStoreState.施工中:
                        stateName = "施工中";
                        break;
                    case MoveStoreState.已完成:
                        stateName = "已完成";
                        break;
                }
                newlist.Add(new Models.GET.MoveStore
                {
                    StateName = stateName,
                    OutPlan = m.OutPlan,
                    OutStoreName = _dbContext.Stores.FirstOrDefault(s => s.Id == m.OutStoreId).Name,
                    OutStoreTypeName = _dbContext.StoreTypes.FirstOrDefault(st => st.Id == m.OutStoreTypeId).Name,
                    InStoreName = _dbContext.Stores.FirstOrDefault(s => s.Id == m.InStoreId).Name,
                    InStoreTypeName = _dbContext.StoreTypes.FirstOrDefault(st => st.Id == m.InStoreTypeId).Name,
                });
            }
            return newlist;
        }
    }
}
