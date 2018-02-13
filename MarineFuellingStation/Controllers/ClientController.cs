using MFS.Controllers.Attributes;
using MFS.Models;
using MFS.Repositorys;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Senparc.Weixin.Work.Containers;
using Senparc.Weixin.Work.AdvancedAPIs;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace MFS.Controllers
{
    [Route("api/[controller]"), Axios]
    public class ClientController : ControllerBase
    {
        private readonly ClientRepository r;
        private readonly UserRepository user_r;
        private readonly IHostingEnvironment _hostingEnvironment;
        WorkOption option;
        public ClientController(ClientRepository repository, UserRepository u_repository, IOptionsSnapshot<WorkOption> option, IHostingEnvironment env)
        {
            r = repository;
            this.user_r = u_repository;
            this.option = option.Value;
            _hostingEnvironment = env;
        }
        #region POST
        [HttpPost]
        public ResultJSON<Client> Post([FromBody]Client model)
        {
            if (r.Has(c => c.CarNo == model.CarNo)) return new ResultJSON<Client> { Code = 0, Msg = "操作失败，已存在" + model.CarNo };
            r.CurrentUser = UserName;
            return new ResultJSON<Client>
            {
                Code = 0,
                Data = r.Insert(model)
            };
        }
        #endregion
        #region GET
        [HttpGet("[action]/{id}")]
        public ResultJSON<Client> GetDetail(int id)
        {
            return new ResultJSON<Client>
            {
                Code = 0,
                Data = r.GetDetail(id)
            };
        }
        /// <summary>
        /// 根据车号/船号获得客户
        /// </summary>
        /// <param name="carNo"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<Client> CreateOrGetClientByCarNo(string carNo)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Client>
            {
                Code = 0,
                Data = r.CreateOrGetByCarNo(carNo)
            };
        }
        [HttpGet]
        public ResultJSON<List<Client>> Get()
        {
            List<Client> ls = r.GetIncludeCompany();
            return new ResultJSON<List<Client>>
            {
                Code = 0,
                Data = ls
            };
        }
        [HttpGet("{sv}")]
        public ResultJSON<List<Client>> Get(string sv)
        {
            List<Client> ls = r.GetIncludeCompany(sv);
            return new ResultJSON<List<Client>>
            {
                Code = 0,
                Data = ls
            };
        }
        /// <summary>
        /// 只根据client表内字段搜索关键字
        /// </summary>
        /// <param name="kw">电话|联系人|船号|车号关键字</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<List<Client>> GetByClientKeyword(string kw)
        {
            List<Client> ls = r.GetByClientKeyword(kw);
            return new ResultJSON<List<Client>>
            {
                Code = 0,
                Data = ls
            };
        }
        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="ctype">客户类型</param>
        /// <param name="ptype">计划状态</param>
        /// <param name="balances">余额条件</param>
        /// <param name="cycle">周期条件</param>
        /// <param name="kw">搜索关键字</param>
        /// <param name="isMy">是否我的客户</param>
        /// <param name="page">第几页</param>
        /// <param name="pageSize">分页记录数</param>
        [HttpGet("[action]")]
        public ResultJSON<List<Client>> GetClients(ClientType ctype, int ptype, int balances, int cycle, string kw, bool isMy, int page, int pageSize)
        {
            r.CurrentUser = UserName;
            PlaceType placeType;
            if (isMy && ctype == ClientType.无销售员)
            {
                user_r.CurrentUser = UserName;
                placeType = PlaceType.水上;
                bool isLand, isWater;
                isLand = user_r.IsInDept("陆上部", this.option);
                isWater = user_r.IsInDept("水上部", this.option);
                if (isLand && isWater)
                    placeType = PlaceType.全部;
                else if (isLand)
                    placeType = PlaceType.陆上;
            }
            else
                placeType = PlaceType.全部;
            return new ResultJSON<List<Client>>
            {
                Code = 0,
                Data = r.GetMyClients(placeType, ctype, ptype, balances, cycle, kw, isMy, page, pageSize)
            };
        }
        [HttpGet("[action]")]
        public ResultJSON<string> ApplyBeMyClient(string carNo, int id, PlaceType placeType)
        {
            try
            {
                string accessToken;
                if (placeType == PlaceType.水上)
                    accessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.水上计划Secret);
                else
                    accessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.陆上计划Secret);
                string agentId = placeType == PlaceType.水上 ? option.水上计划AgentId : option.陆上计划AgentId;

                //推送到“水上或陆上计划”
                MassApi.SendTextCard(accessToken, agentId, $"{UserName}申请{carNo}成为他的客户"
                         , $"<div class=\"gray\">客户：{carNo}</div>"
                         , $"https://vue.car0774.com/#/sales/myclient/{id.ToString()}/{UserName}", toUser: "@all");

                return new ResultJSON<string> { Code = 0, Msg = "提交申请成功" };
            }
            catch
            {
                return new ResultJSON<string> { Code = 503, Msg = "推送失败请重试" };
            }
        }
        [HttpGet("[action]")]
        public ResultJSON<string> ApplyClientToCompany(int cid, int coid, string carNo, string companyName)
        {
            try
            {
                this.option.客户AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.客户Secret);
                //推送到“客户”
                MassApi.SendTextCard(this.option.客户AccessToken, this.option.客户AgentId, $"{UserName}申请{carNo}编入{companyName}"
                         , $"<div class=\"gray\">申请编入公司：{companyName}</div>"
                         , $"https://vue.car0774.com/#/sales/clienttocompany/{cid.ToString()}/{coid.ToString()}/{companyName}", toUser: "@all");
                return new ResultJSON<string> { Code = 0, Msg = "提交申请成功" };
            }
            catch
            {
                return new ResultJSON<string> { Code = 503, Msg = "推送失败请重试" };
            }
        }
        [HttpGet("[action]")]
        public ResultJSON<string> ExportExcel(DateTime start, DateTime end)
        {
            try
            {
                List<Client> list = r.GetAllList(c => c.CreatedAt >= start && c.CreatedAt <= end);
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, @"excel\test.xlsx");
                Helper.FileHelper.ExportExcelByEPPlus(list, new string[] { "Name","CarNo", "FollowSalesman" } ,filePath);
                return new ResultJSON<string> { Code = 0, Data = filePath };
            }
            catch(Exception e)
            {
                return new ResultJSON<string> { Code = 503, Msg = e.Message };
            }
        }
        #endregion
        #region PUT
        /// <summary>
        /// 标记客户
        /// </summary>
        /// <param name="c">客户model</param>
        [HttpPut("[action]")]
        public ResultJSON<Client> MarkTag([FromBody]Client c)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Client>
            {
                Code = 0,
                Data = r.Update(c)
            };
        }
        /// <summary>
        /// 清除我的客户所有标注
        /// </summary>
        /// <param name="c">客户model</param>
        [HttpPut("[action]")]
        public ResultJSON<Client> ClearMyClientMark()
        {
            r.CurrentUser = UserName;
            int count = r.ClearMyClientMark();
            return new ResultJSON<Client>
            {
                Code = 0,
                Msg = "成功更新了" + count.ToString() + "条信息"
            };
        }
        /// <summary>
        /// 提交客户备注信息
        /// </summary>
        /// <param name="c">客户model</param>
        [HttpPut("[action]")]
        public ResultJSON<Client> ReMark([FromBody]Client c)
        {
            r.CurrentUser = UserName;
            return new ResultJSON<Client>
            {
                Code = 0,
                Data = r.Update(c)
            };
        }
        /// <summary>
        /// 把一个或多个客户归入到公司
        /// </summary>
        /// <param name="clientIds"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public ResultJSON<List<Client>> SetClientsToCompany(string clientIds, int companyId)
        {
            List<Client> list = r.SetClientsToCompany(clientIds.Split(','), companyId);
            if (list.Count == 0)
                return new ResultJSON<List<Client>>
                {
                    Code = 503,
                    Msg = "选择的客户有误或存在该公司"
                };
            return new ResultJSON<List<Client>>
            {
                Code = 0,
                Data = list
            };
        }
        /// <summary>
        /// 移除一个或多个客户
        /// </summary>
        /// <param name="clientIds"></param>
        /// <param name="companyId"></param>
        /// <returns>返回删除指定成员后最新成员list</returns>
        [HttpPut("[action]")]
        public ResultJSON<List<Client>> RemoveCompanyClients(string clientIds, int companyId)
        {
            List<Client> list = r.RemoveCompanyClients(clientIds.Split(','), companyId);
            return new ResultJSON<List<Client>>
            {
                Code = 0,
                Data = list
            };
        }
        [HttpPut]
        public ResultJSON<Client> Save([FromBody]Client c)
        {
            try
            {
                return new ResultJSON<Client>
                {
                    Code = 0,
                    Data = r.Update(c)
                };
            }
            catch
            {
                return new ResultJSON<Client> { Code = 503, Msg = "操作失败" };
            }
        }
        #endregion
    }
}
