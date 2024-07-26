

using Data.Model.CustomerModel;
using Data.Model.GemModel;
using Data.Model.ResultModel;
using Data.Model.VoucherModel;
using RTools_NTS.Util;

namespace Bussiness.Services.GemService
{
    public interface IGemService
    {
        public Task<ResultModel> CreateGem(String token, CreateGemModel CreateGemModel);
        Task<ResultModel> UpdateGem(string? token, GemRequestModel gemRequestModel);
        public Task<ResultModel> ViewListGem(string? token, GemSearchModel gemSearch);
    }
}
