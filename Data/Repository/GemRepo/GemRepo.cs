using Data.Entities;
using Data.Model.GemModel;
using Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.GemRepo
{
    public class GemRepo : Repository<Gem>,IGemRepo
    {
        private readonly JewerlyV6Context _context;
        public GemRepo(JewerlyV6Context context) : base(context)
        {
            _context = context;
        }
        public async Task<Gem> CreateGem(Gem gem)
        {
            var lastGem = await _context.Gems
                .OrderByDescending(g => g.GemId)
                .FirstOrDefaultAsync();

            int newIdNumber = 1;
            if (lastGem != null)
            {
                int lastIdNumber;
                if (int.TryParse(lastGem.GemId.Substring(1), out lastIdNumber))
                {
                    newIdNumber = lastIdNumber + 1;
                }
            }
            gem.GemId = $"G{newIdNumber:000}";

            _context.Gems.Add(gem);
            await _context.SaveChangesAsync();
            return gem;
        }

        public async Task<IEnumerable<Gem>> GetGem()
        {
            var gems = await _context.Gems.ToListAsync();
            return gems;
        }

        public async Task<IEnumerable<Gem>> GetGemById()
        {
            return await _context.Gems.ToListAsync();
        }

        public async Task<IEnumerable<Gem>> GetGemByName()
        {
            return await _context.Gems.ToListAsync();
        }
        public async Task<Gem> GetGemByNameAsync(string name) => await _context.Gems.FirstOrDefaultAsync(g => g.Name == name);

        public async Task<Gem> UpdateGemAsync(GemRequestModel gemRequestModel)
        {
            try
            {
                if (gemRequestModel == null || string.IsNullOrEmpty(gemRequestModel.GemId))
                {
                    throw new ArgumentException("Invalid gem data provided.");
                }

                var gem = await _context.Gems.FirstOrDefaultAsync(g => g.GemId == gemRequestModel.GemId);
                if (gem == null)
                {
                    throw new KeyNotFoundException($"Gem with ID {gemRequestModel.GemId} not found.");
                }

                gem.Name = gemRequestModel.Name;
                gem.Type = gemRequestModel.Type;
                gem.Price = gemRequestModel.Price;
                gem.Desc = gemRequestModel.Desc;


                _context.Gems.Update(gem);
                await _context.SaveChangesAsync();

                return gem;
            }
            catch (Exception ex)
             {
                throw new Exception($"Error updating gem: {ex.Message}", ex);
            }
        }
    }
}
