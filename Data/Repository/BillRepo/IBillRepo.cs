using Data.Entities;
using Data.Repository.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.BillRepo
{
    public interface IBillRepo : IRepository<Bill>
    {
        IQueryable<Bill> GetBillQuery();
        Task<decimal> TotalBill();
    }
}
