using Data.Entities;
using Data.Repository.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.DiscountRepo
{
    public interface IDiscountRepo : IRepository<Discount>
    {
        Task<List<Discount>> GetAllDiscount();
        Task<List<Discount>> GetActiveDiscount(DateTime expiredDate);
        Task<Discount> GetDiscountById(string id);
        Task UpdateDiscount(Discount discount);
    }
}
