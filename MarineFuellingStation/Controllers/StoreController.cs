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
    public class StoreController : ControllerBase
    {
        private readonly StoreRepository r;
        WorkOption option;
        public StoreController(StoreRepository repository, IOptionsSnapshot<WorkOption> option)
        {
            r = repository;
            //获取 系统设置 企业微信应用的AccessToken
            this.option = option.Value;
            this.option.系统设置AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.系统设置Secret);
        }
        #region GET操作
        [HttpGet]
        public ResultJSON<List<Store>> Get()
        {
            return new ResultJSON<List<Store>>
            {
                Code = 0,
                Data = r.GetAllList().OrderBy(s => s.Name).ToList()
            };
        }
        [HttpGet("{id}")]
        public ResultJSON<Store> Get(int id)
        {
            Store s = r.Get(id);
            return new ResultJSON<Store>
            {
                Code = 0,
                Data = s
            };
        }
        [HttpGet("[action]")]
        public ResultJSON<List<Store>> GetByStoreType(int stypeId)
        {
            return new ResultJSON<List<Store>>
            {
                Code = 0,
                Data = r.GetAllList(s => s.StoreTypeId == stypeId).OrderBy(s => s.Name).ToList()
            };
        }
        /// <summary>
        /// 根据油仓类型获取数据
        /// </summary>
        /// <param name="sc">销售仓/仓储仓</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ResultJSON<List<Store>> GetByClass(StoreClass sc)
        {
            return new ResultJSON<List<Store>>
            {
                Code = 0,
                Data = r.GetByClass(sc)
            };
        }
        #endregion
        #region POST操作
        [HttpPost]
        public ResultJSON<Store> Post([FromBody]Store model)
        {
            if (r.Has(s => s.Name == model.Name)) return new ResultJSON<Store> { Code = 0, Msg = "操作失败，已存在" + model.Name };
            r.CurrentUser = UserName;
            //推送到“系统设置”
            MassApi.SendTextCard(option.系统设置AccessToken, option.系统设置AgentId, $"{UserName}新增了{model.Name}"
                     , $"<div class=\"gray\">时间：{DateTime.Now.ToString("yyyy-MM-dd hh:mm")}</div>"
                     , "https://vue.car0774.com/#/oilstore/store", toUser: "@all");
            return new ResultJSON<Store>
            {
                Code = 0,
                Data = r.Insert(model)
            };
        }
        /// <summary>
        /// 批量更新油仓油量，密度
        /// </summary>
        /// <param name="list">Stores</param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ResultJSON<string> PostStores([FromBody]List<Store> list)
        {
            int count = 0;
            if (list.Count > 0)
            {   
                foreach(Store st in list)
                {
                    try
                    {
                        count++;
                        r.Update(st);
                    }
                    catch
                    {
                        count--;
                    }
                }
            }
            return new ResultJSON<string>
            {
                Code = 0,
                Msg = "成功初始化了" + count.ToString() + "个油仓"
            };
        }
        #endregion
        #region PUT操作
        [HttpPut]
        public ResultJSON<Store> Put([FromBody]Store model)
        {
            r.CurrentUser = UserName;
            //推送到“系统设置”
            MassApi.SendTextCard(option.系统设置AccessToken, option.系统设置AgentId, $"{UserName}修改了{model.Name}"
                     , $"<div class=\"gray\">时间：{DateTime.Now.ToString("yyyy-MM-dd hh:mm")}</div>"
                     , "https://vue.car0774.com/#/oilstore/store", toUser: "@all");
            return new ResultJSON<Store>
            {
                Code = 0,
                Data = r.InsertOrUpdate(model)
            };
        }
        #endregion
    }
}
