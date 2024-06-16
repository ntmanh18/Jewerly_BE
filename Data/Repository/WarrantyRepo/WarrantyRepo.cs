using Data.Entities;
using Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.WarrantyRepo
{
    public class WarrantyRepo : Repository<Warranty>, IWarrantyRepo
    {
        private readonly JewerlyV6Context _context;
        public WarrantyRepo(JewerlyV6Context context) : base(context)
        {
            _context = context;
        }

        public async Task<Warranty> CreateWarrantyAsync(Warranty warranty)
        {
            if (warranty.Desc == "One-year warranty")
            {
                warranty.ExpiredDate = warranty.StartDate.AddYears(1);
            }
            else if (warranty.Desc == "Half-year warranty")
            {
                warranty.ExpiredDate = warranty.StartDate.AddMonths(6);
            }
            else if (warranty.Desc == "Two-year warranty")
            {
                warranty.ExpiredDate = warranty.StartDate.AddYears(2);
            }
            _context.Warranties.Add(warranty);
            await _context.SaveChangesAsync();
            return warranty;
        }

        public async Task<Warranty> GetLastWarrantyAsync()
        {
            return await _context.Warranties
                .OrderByDescending(v => v.WarrantyId)
                .FirstOrDefaultAsync();
        }

        public async Task<Warranty> GetWarrantyByIdWithIncludesAsync(string WarrantyID)
        {
            return await _context.Warranties
            .Include(v => v.CustomerCustomer).Include(v => v.Product).FirstOrDefaultAsync(v => v.WarrantyId == WarrantyID);
        }
    }
}
