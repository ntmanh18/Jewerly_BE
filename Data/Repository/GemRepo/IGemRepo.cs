

using Data.Entities;
using Data.Model.GemModel;

namespace Data.Repository.GemRepo
{
    public interface IGemRepo
    {
        public Task<Gem> CreateGem(Gem gem);
        public Task<IEnumerable<Gem>> GetGem();
        Task<IEnumerable<Gem>> GetGemByName();
        Task<IEnumerable<Gem>> GetGemById();
        Task<Gem> UpdateGemAsync(GemRequestModel gemRequestModel);
        Task<Gem> GetGemByNameAsync(string name);
    }
}
