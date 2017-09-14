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
                switch (value)
                {
                    case 1:
                        Msg = "文件没有内容";
                        break;
                    case 0:
                        Msg = "请求成功";
                        break;
                    case -1:
                        Msg = "系统繁忙";
                        break;
                }
                _code = value;
            }
        }
        public string Msg { get; set; }
        public T Data { get; set; }
    }
}
