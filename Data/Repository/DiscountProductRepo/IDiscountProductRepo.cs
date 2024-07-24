using Data.Entities;
using Data.Repository.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.DiscountProductRepo
{
    public interface IDiscountProductRepo : IRepository<DiscountProduct>    
    {
        Task DeleteDiscountProduct(Discount discount,Product product);
        Task CreateDiscountProduct(Discount discount, Product product);
    }
}
