

using Data.Entities;
using Data.Model.GemModel;

namespace Data.Repository.GemRepo
{
    public interface IGemRepo
       {
        public Task<Gem> CreateGem(Gem gem);
        IQueryable<Gem> GetVoucherQuery();
        Task<Gem> GetGemById(string id);
        Task<Gem> UpdateGem(Gem gemRequestModel);
        Task<Gem> UpdateGemAsync(GemRequestModel gemRequestModel);
        Task<Gem> GetGemByName(string name);
    }
}
