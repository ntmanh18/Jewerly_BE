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

        public async Task<Gem> GetGemById(string id)
        {
            return await _context.Gems.FirstOrDefaultAsync(c => c.GemId == id);
        }
        public async Task<Gem> GetGemByName(string name) => await _context.Gems.FirstOrDefaultAsync(g => g.Name == name);
        public async Task<Gem> UpdateGemAsync(GemRequestModel gemRequestModel)
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

            if (!string.IsNullOrEmpty(gemRequestModel.Name))
            {
                gem.Name = gemRequestModel.Name;
            }

            if (gemRequestModel.Type == 1 || gemRequestModel.Type == 2)
            {
                gem.Type = gemRequestModel.Type;
            }

            if (gemRequestModel.Price != 0L)
            {
                gem.Price = gemRequestModel.Price;
            }

            if (!string.IsNullOrEmpty(gemRequestModel.Desc))
            {
                gem.Desc = gemRequestModel.Desc;
            }
            if (gemRequestModel.rate >= 0 && gemRequestModel.rate <= 1)
            {
                gem.Rate = gemRequestModel.rate;
            }
            return gem;
        }
        public async Task<Gem> UpdateGem(Gem gemRequestModel)
        {
            try
            {
                _context.Gems.Update(gemRequestModel);
                await _context.SaveChangesAsync();

                return gemRequestModel;
            }
            catch (Exception ex)
             {
                throw new Exception($"Error updating gem: {ex.Message}", ex);
            }
        }
    }
}
