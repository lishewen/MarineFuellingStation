using Senparc.Weixin.Work.AdvancedAPIs.MailList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    public class UserDTO
    {
        /// <summary>
        /// 企业微信部分信息
        /// </summary>
        public GetMemberResult WorkInfo { get; set; }
        /// <summary>
        /// 本地数据库信息
        /// </summary>
        public User LocalInfo { get; set; }
    }
}
