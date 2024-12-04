using Data.Model.CustomerModel;
using Data.Model.GoldModel;
using Data.Model.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.GoldService
{
    public interface IGoldService
    {
        public Task<ResultModel> GetGolds(string token);
        Task<ResultModel> GetGoldById(string? token, string id);
        public Task<ResultModel> CreateGold(string token, GoldCreateModel goldModel, string userId);
        Task<ResultModel> UpdateGold(string? token, GoldUpdateModel goldModel);

        Task<ResultModel> DeleteListGold(string token);

    }
}
