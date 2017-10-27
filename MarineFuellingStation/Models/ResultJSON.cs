using Newtonsoft.Json;
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
                    { 500, "扣减金额必须少于或等于账户余额" }
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
            return JsonConvert.SerializeObject(this);
        }
    }
}
