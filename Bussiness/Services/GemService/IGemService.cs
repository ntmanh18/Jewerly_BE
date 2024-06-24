

using Data.Model.CustomerModel;
using Data.Model.GemModel;
using Data.Model.ResultModel;
using RTools_NTS.Util;

namespace Bussiness.Services.GemService
{
    public interface IGemService
    {
        public Task<ResultModel> CreateGem(String token, CreateGemModel CreateGemModel);
        public Task<IEnumerable<GemRequestModel>> GetGem();
        Task<ResultModel> GetGemByName(string? token, string gemName);
        Task<ResultModel> GetGemById(string? token, string gemId);
        Task<ResultModel> UpdateGem(string? token, GemRequestModel gemRequestModel);
    }
}
