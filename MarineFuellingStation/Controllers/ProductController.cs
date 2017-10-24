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
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository r;
        WorkOption option;
        public ProductController(ProductRepository repository, IOptionsSnapshot<WorkOption> option)
        {
            r = repository;
            //获取 系统设置 企业微信应用的AccessToken
            this.option = option.Value;
            this.option.系统设置AccessToken = AccessTokenContainer.TryGetToken(this.option.CorpId, this.option.系统设置Secret);
        }
        [HttpGet("[action]")]
        public ResultJSON<List<Product>> OilProducts()
        {
            return new ResultJSON<List<Product>>
            {
                Code = 0,
                Data = r.GetAllList((p) => p.ProductType.Name == "销售油" && p.IsUse)
            };
        }
        [HttpGet("[action]")]
        public ResultJSON<List<Product>> PurchaseOilProducts()
        {
            return new ResultJSON<List<Product>>
            {
                Code = 0,
                Data = r.GetAllList((p) => p.ProductType.Name == "采购油" && p.IsUse)
            };
        }
        [HttpGet("{id}")]
        public ResultJSON<Product> Get(int id)
        {
            Product s = r.Get(id);
            return new ResultJSON<Product>
            {
                Code = 0,
                Data = s
            };
        }
        [HttpPut]
        public ResultJSON<Product> Put([FromBody]Product model)
        {
            r.CurrentUser = UserName;
            //推送到“系统设置”
            MassApi.SendTextCard(option.系统设置AccessToken, option.系统设置AgentId, $"{UserName}修改了{model.Name}"
                     , $"<div class=\"gray\">时间：{DateTime.Now.ToString("yyyy-MM-dd hh:mm")}</div>" 
                     , "https://vue.car0774.com/#/oilstore/product", toUser: "@all");
            return new ResultJSON<Product>
            {
                Code = 0,
                Data = r.InsertOrUpdate(model)
            };
        }
        /// <summary>
        /// 新增POST
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ResultJSON<Product> Post([FromBody]Product model)
        {
            r.CurrentUser = UserName;
            //推送到“系统设置”
            MassApi.SendTextCard(option.系统设置AccessToken, option.系统设置AgentId, $"{UserName}新增了商品{model.Name}"
                     , $"<div class=\"gray\">时间：{DateTime.Now.ToString("yyyy-MM-dd hh:mm")}</div>"
                     , "https://vue.car0774.com/#/oilstore/product", toUser: "@all");
            return new ResultJSON<Product>
            {
                Code = 0,
                Data = r.Insert(model)
            };
        }
    }
}
