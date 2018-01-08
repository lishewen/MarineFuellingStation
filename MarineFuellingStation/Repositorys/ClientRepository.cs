using MFS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace MFS.Repositorys
{
    public class ClientRepository : RepositoryBase<Client>
    {
        public ClientRepository(EFContext dbContext) : base(dbContext) { }
        public List<Client> GetIncludeCompany()
        {
            return _dbContext.Clients.Include("Company").ToList();
        }
        public List<Client> GetIncludeCompany(string sv)
        {
            return _dbContext.Clients.Include("Company").Where(s => 
                    s.CarNo.Contains(sv)
                    || s.Company.Name.Contains(sv)
                    || s.Contact.Contains(sv)).ToList();
        }
        public Client GetDetail(int id)
        {
            return _dbContext.Clients.Include("Company").Include("Product").FirstOrDefault(c => c.Id == id);
        }
        /// <summary>
        /// 根据车号/船号获得客户
        /// </summary>
        /// <param name="carNo"></param>
        /// <returns></returns>
        public Client CreateOrGetByCarNo(string carNo)
        {
            if (Has(c => c.CarNo == carNo))
                return _dbContext.Clients.Include("Company").FirstOrDefault(c => c.CarNo == carNo);
            else
                return Insert(new Client {Name = "个人", CarNo = carNo, FollowSalesman = CurrentUser });
        }
        /// <summary>
        /// 只根据client表内字段搜索关键字
        /// </summary>
        /// <param name="kw">电话|联系人|船号|车号关键字</param>
        /// <returns></returns>
        public List<Client> GetByClientKeyword(string kw)
        {
            return _dbContext.Clients.Where(c =>
                c.Mobile == kw
                || c.Phone == kw
                || c.CarNo.Contains(kw)
                || c.Contact == kw).Include("Company").ToList();
        }
        public List<Client> GetMyClients(PlaceType placeType, ClientType ctype, int ptype, int balances, int cycle, string kw, bool isMy, int page, int pageSize)
        {
            List<Client> list;

            Expression<Func<Client, bool>> clientwhere = c => 1 == 1;
            if (placeType == PlaceType.水上 || placeType == PlaceType.陆上)
                clientwhere = c => c.PlaceType == placeType;

            if(isMy && ctype != ClientType.无销售员)
                clientwhere = clientwhere.And(c => c.FollowSalesman == CurrentUser);

            if (ptype > 0)//计划状态
            {
                var sts = (SalesPlanState)ptype;
                var clist = _dbContext.SalesPlans.Where(s => s.State == sts).Select(s => s.CarNo);
                clientwhere = clientwhere.And(c => clist.Contains(c.CarNo));
            }

            if (balances > 0)//余额
                clientwhere = clientwhere.And(c => c.Balances < balances);

            if (cycle > 0)//周期
            {
                var cylist = _dbContext.SalesPlans.Where(s => (DateTime.Now - s.LastUpdatedAt).Days < cycle && s.CreatedBy == CurrentUser).Select(s => s.CarNo).ToList();
                clientwhere = clientwhere.And(c => !cylist.Contains(c.CarNo));
            }
            //客户类型：个人，公司，全部，无销售员
            if (ctype == ClientType.全部)
                clientwhere = clientwhere.And(c => (c.ClientType == ClientType.个人 || c.ClientType == ClientType.公司));
            else if (ctype == ClientType.无销售员)
                clientwhere = clientwhere.And(c => c.FollowSalesman == "" || c.FollowSalesman == null);
            else
                clientwhere = clientwhere.And(c => c.ClientType == ctype);

            if (!string.IsNullOrEmpty(kw))
                clientwhere = clientwhere.And(c => c.CarNo.Contains(kw));
            list = LoadPageList(page, pageSize, out int rowCount, true, false, clientwhere).Include("Company").ToList();
            //list = _dbContext.Clients.Include("Company").Where(clientwhere).ToList();

            return list;
        }
        /// <summary>
        /// 更新客户默认商品
        /// </summary>
        /// <param name="carNo"></param>
        /// <param name="defaultProductId"></param>
        /// <returns></returns>
        public bool SaveDefaultProduct(string carNo, int defaultProductId)
        {
            try
            {
                Client c = _dbContext.Clients.FirstOrDefault(cl => cl.CarNo == carNo);
                if (c != null)
                {
                    c.DefaultProductId = defaultProductId;
                    Save();
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 把一个或多个客户归入到公司
        /// </summary>
        /// <param name="aClientIds"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public List<Client> SetClientsToCompany(string[] aClientIds, int companyId)
        {
            List<Client> list = new List<Client>();
            foreach(string cid in aClientIds)
            {
                Client c = _dbContext.Clients.Where(cl => cl.Id == int.Parse(cid)).Include("Company").FirstOrDefault();
                if (c.CompanyId != companyId)
                {
                    c.CompanyId = companyId;
                    Save();
                    list.Add(c);
                }
                else
                    continue;
            }
            return list;
        }
        /// <summary>
        /// 移除一个或多个公司下的成员
        /// </summary>
        /// <param name="aClientIds"></param>
        /// <param name="companyId"></param>
        /// <returns>返回删除指定成员后最新成员list</returns>
        public List<Client> RemoveCompanyClients(string[] aClientIds, int companyId)
        {
            foreach (string cid in aClientIds)
            {
                Client c = _dbContext.Clients.Where(cl => cl.Id == int.Parse(cid)).Include("Company").FirstOrDefault();
                if (c.CompanyId == companyId)
                {
                    c.CompanyId = null;
                    Save();
                }
                else
                    continue;
            }
            return _dbContext.Clients.Where(cl => cl.CompanyId == companyId && !cl.IsDel).ToList();
        }
        
    }
}
