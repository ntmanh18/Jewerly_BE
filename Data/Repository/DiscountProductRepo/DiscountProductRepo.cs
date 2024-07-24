using Data.Entities;
using Data.Repository.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.DiscountProductRepo
{
    public class DiscountProductRepo : Repository<DiscountProduct>, IDiscountProductRepo
    {
        private readonly JewerlyV6Context _context;
        public DiscountProductRepo(JewerlyV6Context context) :base(context) 
        {
            _context = context;
        }
        public Task CreateDiscountProduct(Discount discount, Product product)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDiscountProduct(Discount discount, Product product)
        {
            throw new NotImplementedException();
        }
    }
}
