using MFS.Controllers.Attributes;
using MFS.Models;
using MFS.Repositorys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Senparc.Weixin.Work.AdvancedAPIs;
using Senparc.Weixin.Work.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Controllers
{
    [Route("api/[controller]"), Axios]
    public class WageController : ControllerBase
    {
        private readonly UserRepository r;
        private readonly WageRepository wr;
        private readonly OrderRepository orderrepository;
        WorkOption option;
        public WageController(UserRepository repository, WageRepository wageRepository, OrderRepository orderRepository, IOptionsSnapshot<WorkOption> option)
        {
            r = repository;
            wr = wageRepository;
            orderrepository = orderRepository;

            this.option = option.Value;
            this.option.AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.Secret);
        }
        [HttpGet("[action]/{ym}/{departid}")]
        public ResultJSON<List<Wage>> GetByDepart(string ym, string departid)
        {
            int d = Convert.ToInt32(ym);

            var ids = departid.Split('|');

            List<int> items = new List<int>();
            foreach (var id in ids)
                items.Add(Convert.ToInt32(id));

            return new ResultJSON<List<Wage>>
            {
                Code = 0,
                Data = wr.GetAllList(w => w.年月 == d && items.Contains(w.DepartmentId))
            };
        }
        [HttpGet("{ym?}")]
        public async Task<ResultJSON<List<Wage>>> Get(string ym)
        {
            if (string.IsNullOrWhiteSpace(ym))
                ym = DateTime.Now.ToString("yyyyMM");

            int d = Convert.ToInt32(ym);
            int y = Convert.ToInt32(ym.Substring(0, 4));
            int m = Convert.ToInt32(ym.Substring(4, 2));

            var users = r.GetAllList();
            foreach (var u in users)
            {
                var workInfo = await MailListApi.GetMemberAsync(option.AccessToken, u.UserId);

                DateTime startdate = new DateTime(y, m, 1, 0, 0, 0);
                DateTime enddate = startdate.AddMonths(1).AddSeconds(-1);

                var olist = orderrepository.GetAllList(o => o.PayState == PayState.已结算
                                                        && o.CreatedBy == u.Name
                                                        && o.LastUpdatedAt >= startdate && o.LastUpdatedAt <= enddate);


                if (!wr.Has(w => w.年月 == d && w.Name == u.Name))
                {
                    var wage = new Wage
                    {
                        Name = u.Name,
                        年月 = d,
                        基本 = u.BaseWage,
                        社保 = u.SocialSecurity,
                        安全保障金 = u.Security,
                        职务 = workInfo.isleader == 0 ? "普通职员" : "上级",
                        DepartmentId = workInfo.department[0]
                    };

                    if (olist.Count > 0)
                    {
                        wage.超额 = olist.Sum(o => (o.Price - o.MinPrice) * o.Count);
                        wage.提成 = olist.Sum(o => o.SalesCommission);
                    }

                    wr.Insert(wage);
                }
                else
                {
                    var wage = wr.FirstOrDefault(w => w.年月 == d && w.Name == u.Name);
                    wage.Name = u.Name;
                    wage.年月 = d;
                    wage.基本 = u.BaseWage;
                    wage.社保 = u.SocialSecurity;
                    wage.安全保障金 = u.Security;
                    wage.职务 = workInfo.isleader == 0 ? "普通职员" : "上级";
                    wage.DepartmentId = workInfo.department[0];

                    if (olist.Count > 0)
                    {
                        wage.超额 = olist.Sum(o => (o.Price - o.MinPrice) * o.Count);
                        wage.提成 = olist.Sum(o => o.SalesCommission);
                    }

                    wr.Save();
                }
            }

            var list = wr.GetAllList(w => w.年月 == d);
            foreach (var wage in list)
                wage.实发 = wage.基本 + wage.提成 + wage.交通 - wage.社保 - wage.安全保障金 - wage.请假 - wage.餐费 - wage.借支;

            return new ResultJSON<List<Wage>>
            {
                Code = 0,
                Data = list
            };
        }

        [HttpPost]
        public ResultJSON<Wage> Post([FromBody]Wage wage)
        {
            r.FirstOrDefault(u => u.Name == wage.Name).Security = wage.安全保障金;

            return new ResultJSON<Wage>
            {
                Code = 0,
                Data = wr.InsertOrUpdate(wage)
            };
        }
    }
}
