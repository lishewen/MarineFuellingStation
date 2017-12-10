using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    public class ResultJSON<T>
    {
        private int _code;
        public int Code
        {
            get
            {
                return _code;
            }
            set
            {
                //统一错误代号提示
                Dictionary<int, string> dict = new Dictionary<int, string> {
                    { 1, "文件没有内容" },
                    { 0, "请求成功" },
                    { -1, "系统繁忙" },
                    {502, "单号重复，请重新提交" }
                };

                if (dict.Keys.Contains(value))
                    Msg = dict[value];

                _code = value;
            }
        }
        public string Msg { get; set; }
        public T Data { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }
}
