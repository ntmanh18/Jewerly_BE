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
        Task<List<DiscountProduct>> GetAllDiscountProduct();
        Task<List<DiscountProduct>> GetDiscountProductByDiscout(string discountId);
        Task<List<DiscountProduct>> GetDiscountProductByProduct(string productId);

        Task DeleteDiscountProduct(DiscountProduct discountProduct);
        Task CreateDiscountProduct(DiscountProduct discountProduct);
        Task<DiscountProduct> GetUniqueDiscountProduct(string discountId, string productId);    
    }
}
