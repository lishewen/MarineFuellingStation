using MFS.Controllers.Attributes;
using MFS.Models;
using MFS.Repositorys;
using Microsoft.AspNetCore.Mvc;
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
        public WageController(UserRepository repository, WageRepository wageRepository, OrderRepository orderRepository)
        {
            r = repository;
            wr = wageRepository;
            orderrepository = orderRepository;
        }
        [HttpGet("{ym?}")]
        public ResultJSON<List<Wage>> Get(string ym)
        {
            if (string.IsNullOrWhiteSpace(ym))
                ym = DateTime.Now.ToString("yyyyMM");

            int d = Convert.ToInt32(ym);
            int y = Convert.ToInt32(ym.Substring(0, 4));
            int m = Convert.ToInt32(ym.Substring(3, 2));

            var users = r.GetAllList();
            foreach (var u in users)
            {
                var wage = new Wage
                {
                    Name = u.Name,
                    基本 = u.BaseWage,
                    社保 = u.SocialSecurity,
                    安全保障金 = u.Security
                };

                DateTime startdate = new DateTime(y, m, 1, 0, 0, 0);
                DateTime enddate = startdate.AddMonths(1).AddSeconds(-1);

                var olist = orderrepository.GetAllList(o => o.PayState == PayState.已结算
                                                        && o.CreatedBy == u.Name
                                                        && o.LastUpdatedAt >= startdate && o.LastUpdatedAt <= enddate);

                if (olist.Count > 0)
                {
                    wage.超额 = olist.Sum(o => (o.Price - o.MinPrice) * o.Count);
                    wage.提成 = olist.Sum(o => o.SalesCommission);
                }

                //wage.实发 = wage.基本 + wage.提成 + wage.交通 - wage.社保 - wage.安全保障金 - wage.请假 - wage.餐费 - wage.借支;

                if (!wr.Has(w => w.年月 == d && w.Name == u.Name))
                {
                    wr.Insert(wage);
                }
                else
                {
                    wr.Update(w => w.年月 == d && w.Name == u.Name, wage);
                }
            }

            return new ResultJSON<List<Wage>>
            {
                Code = 0,
                Data = wr.GetAllList(w => w.年月 == d)
            };
        }
    }
}
