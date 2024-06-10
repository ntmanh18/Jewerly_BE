using Data.Entities;
using Data.Model.OldProductModel;
using Data.Model.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.OldProductService
{
    public interface IOldProductService
    {
        public Task<ResultModel> GetAllAsync(string token);
        Task<ResultModel> GetByIdAsync(string token, string id);
        Task<ResultModel> GetByProductIdAsync(string token, string productId);

    }
}
