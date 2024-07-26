using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.WarrantyRepo
{
    public interface IWarrantyRepo
    {
        public Task<Warranty> CreateWarrantyAsync(Warranty warranty);
        public Task<Warranty> GetWarrantyByIdWithIncludesAsync(string WarrantyID);
        public Task<Warranty> GetLastWarrantyAsync();
        public Task<Warranty> UpdateWarrantyAsync(Warranty warrantyUpdate);
        public Task<Warranty> GetWarrantyByIdAsync(string warrantyId);
        IQueryable<Warranty> GetWarrantyQuery();

    }
}
