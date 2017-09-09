using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class CompanyRepository : RepositoryBase<Company>
    {
        public CompanyRepository(EFContext dbContext) : base(dbContext) { }
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public new Company Insert(Company entity, bool autoSave = true)
        {
            var p = _dbContext.Companys.Find(entity.Id);
            return base.Insert(entity, autoSave);
        }
    }
}
