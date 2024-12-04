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

        public async Task<IEnumerable<Gold>> GetGolds()
        {
            var golds = await _context.Golds.ToListAsync();
            return golds;
        }
        public async Task<Gold> GetGoldById(string id)
        {
            return await _context.Golds.FirstOrDefaultAsync(x => x.GoldId == id);
        }

        public async Task CreateGold(Gold goldFilter)
        {
            await _context.AddAsync(goldFilter);
            _context.SaveChanges();
        }
        public async Task<User?> GetByIdAsync(string id)
        {
            return _context.Users.FirstOrDefault(c => c.UserId == id);
        }
        public async Task<Gold> UpdateGold(Gold goldUpdate)
        {
            try
            {
                _context.Update(goldUpdate);
                await _context.SaveChangesAsync();
                return goldUpdate;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
    }
}
