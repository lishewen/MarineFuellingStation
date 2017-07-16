using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    public class ResultJSON<T>
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public T Data { get; set; }
    }
}
