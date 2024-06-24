

using Data.Entities;
using Data.Model.GemModel;

namespace Data.Repository.GemRepo
{
    public interface IGemRepo
       {
        public Task<Gem> CreateGem(Gem gem);
        public Task<IEnumerable<Gem>> GetGem(); 
        Task<Gem> GetGemByName(string name);
        Task<Gem> GetGemById(string id);
        Task<Gem> UpdateGemAsync(GemRequestModel gemRequestModel);
    }
}
