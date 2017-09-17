using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models.GET
{
    public class MoveStore
    {
        public string OutStoreTypeName { get; set; }
        public string InStoreTypeName { get; set; }
        public string OutStoreName { get; set; }
        public string InStoreName { get; set; }
        public string StateName { get; set; }
        public decimal OutPlan { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
