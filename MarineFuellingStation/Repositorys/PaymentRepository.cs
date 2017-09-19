using MFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Repositorys
{
    public class PaymentRepository : RepositoryBase<Payment>
    {
        public PaymentRepository(EFContext dbContext) : base(dbContext) { }
    }
}
