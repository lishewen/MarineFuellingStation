using MFS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class BoatCleanRepository : RepositoryBase<BoatClean>
    {
        const string tag = "QW";
        public BoatCleanRepository(EFContext dbContext) : base(dbContext) { }
        public string GetLastBoatCleanNo()
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
        /// 根据BoatCleanPayState获取分页数据
        /// </summary>
        /// <param name="payState">付款状态</param>
        /// <param name="startPage">第N页</param>
        /// <param name="pageSize">每页记录</param>
        /// <returns></returns>
        public List<BoatClean> GetByPayState(BoatCleanPayState payState, int startPage, int pageSize, string searchVal)
        {
           Expression<Func<BoatClean, bool>> bcwhere = b => b.PayState == payState;
            if (!string.IsNullOrEmpty(searchVal))
                bcwhere = bcwhere.And(b => b.Name.Contains(searchVal) || b.CarNo.Contains(searchVal));

            return LoadPageList(startPage, pageSize, out int count, true, false, bcwhere)
                .OrderByDescending(o => o.LastUpdatedAt).ToList();
        }
        public BoatClean Pay(BoatClean model)
        {
            foreach(Payment p in model.Payments)
            {
                _dbContext.Payments.Add(p);
            }
            return Update(model);
        }
        
    }
}
