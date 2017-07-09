using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    public class WorkOption
    {
        public string Token { get; set; }
        public string EncodingAESKey { get; set; }
        public string CorpId { get; set; }
        public string Secret { get; set; }
        public string AccessToken { get; set; }
        public string AgentId { get; set; }
    }
}
