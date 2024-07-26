using Azure;
using Data.Entities;
using Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.DiscountRepo
{
    public class DiscountRepo : Repository<Discount>, IDiscountRepo
    {
        private readonly JewerlyV6Context _context;
        public DiscountRepo(JewerlyV6Context context) :base(context)
        {
            _context = context;
        }



        public Task<List<Discount>> GetActiveDiscount(DateTime expiredDate)
        {
            throw new NotImplementedException();
            
        }

        public async Task<List<Discount>> GetAllDiscount()
        {
            return await _context.Discounts.Include(c => c.DiscountProducts).Include(c=> c.CreatedByNavigation).Select( c=> new Discount
            {
               DiscountId = c.DiscountId,
               CreatedBy = c.CreatedByNavigation.UserId,
               ExpiredDay = c.ExpiredDay,
               PublishDay = c.PublishDay,
               Cost = c.Cost,
               CreatedByNavigation = c.CreatedByNavigation,
               DiscountProducts = c.DiscountProducts

            }).AsQueryable().ToListAsync();
        }

        public async Task<Discount> GetDiscountById(string id)
        {
            return await _context.Discounts.Include(c => c.DiscountProducts).FirstOrDefaultAsync(c => c.DiscountId ==id);
            }

        public async Task UpdateDiscount(Discount discount)
        {
            _context.Discounts.Update(discount);
            await _context.SaveChangesAsync();
        }
    }
}
