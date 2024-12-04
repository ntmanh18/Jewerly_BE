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

        public async Task DeleteDiscountProduct(Discount discount)
        {
            try
            {
                var existingDiscount = await _context.Discounts
                                        .Include(x => x.ProductProducts)
                                        .FirstOrDefaultAsync(x => x.DiscountId == discount.DiscountId);

                if (existingDiscount != null)
                {
                    //Update the existing discount values
                    _context.Entry(existingDiscount).CurrentValues.SetValues(discount);

                    //Ensure the ProductProducts collection is not null
                    if (existingDiscount.ProductProducts != null)
                    {
                        //Remove the existing relationships
                        foreach (var productProduct in existingDiscount.ProductProducts.ToList())
                        {
                            _context.Entry(existingDiscount).State = EntityState.Deleted;
                        }
                    }

                    //Save changes
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Discount not found.");
                }
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while saving the entity changes.", e);
            }
        }


        public Task<List<Discount>> GetActiveDiscount(DateTime expiredDate)
        {
            throw new NotImplementedException();
            
        }

        public async Task<List<Discount>> GetAllDiscount()
        {
            return await _context.Discounts.Include(c => c.ProductProducts).Include(c=> c.CreatedByNavigation).Select( c=> new Discount
            {
               DiscountId = c.DiscountId,
               CreatedBy = c.CreatedByNavigation.UserId,
               ExpiredDay = c.ExpiredDay,
               PublishDay = c.PublishDay,
               Cost = c.Cost,
               CreatedByNavigation = c.CreatedByNavigation,
               ProductProducts = c.ProductProducts

            }).AsQueryable().ToListAsync();
        }

        public async Task<Discount> GetDiscountById(string id)
        {
            return await _context.Discounts.Include(c => c.ProductProducts).FirstOrDefaultAsync(c => c.DiscountId ==id);
            }

        public async Task UpdateDiscount(Discount discount)
        {
            _context.Discounts.Update(discount);
            await _context.SaveChangesAsync();
        }
    }
}
