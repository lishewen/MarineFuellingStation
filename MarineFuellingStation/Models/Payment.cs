using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    public class Payment: EntityBase
    {
        public OrderPayType PayTypeId { get; set; }
    }
}
