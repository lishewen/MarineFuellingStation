using MFS.Models;
using Senparc.Weixin.Work.AdvancedAPIs;
using Senparc.Weixin.Work.AdvancedAPIs.MailList;
using Senparc.Weixin.Work.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(EFContext dbContext) : base(dbContext) { }
        /// <summary>
        /// 当前用户是否在指定部门
        /// </summary>
        /// <param name="deptName">部门名称，可以是子部门</param>
        /// <returns>true|false</returns>
        public bool IsInDept(string deptName, WorkOption option)
        {
            if(string.IsNullOrEmpty(option.AccessToken))
                option.AccessToken = AccessTokenContainer.TryGetToken(option.CorpId, option.Secret);
            GetDepartmentMemberInfoResult membersinfo = MailListApi.GetDepartmentMemberInfo(option.AccessToken, 1, 1);
            GetMemberResult user = membersinfo.userlist.FirstOrDefault(m => m.name == CurrentUser);
            GetDepartmentListResult depts = MailListApi.GetDepartmentList(option.AccessToken);
            long landDeptId = depts.department.First(d => d.name == deptName).id;
            foreach (var deptId in user.department)
            {
                if (deptId == landDeptId)
                    return true;
            }
            return false;
        }
        public GetTagMemberResult GetTagMember(string tagname, WorkOption option)
        {
            if (string.IsNullOrEmpty(option.AccessToken))
                option.AccessToken = AccessTokenContainer.TryGetToken(option.CorpId, option.Secret);
            var listresult = MailListApi.GetTagList(option.AccessToken);
            var tagid = listresult.taglist.FirstOrDefault(t => t.tagname == tagname).tagid;
            return MailListApi.GetTagMember(option.AccessToken, Convert.ToInt32(tagid));
        }
    }
}
