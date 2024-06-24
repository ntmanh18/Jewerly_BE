using Data.Entities;
using Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.GoldRepo
{
    public class GoldRepo : Repository<Gold>, IGoldRepo
    {
        private readonly JewerlyV6Context _context;
        public GoldRepo(JewerlyV6Context context) : base(context) 
        {
            _context = context;
        }
        public async Task<Gold> GetGoldById(string id)
        {
            return await _context.Golds.FirstOrDefaultAsync(x => x.GoldId == id);
        }
    }
}
