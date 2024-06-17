using Data.Model.ResultModel;
using Data.Model.WarrantyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.WarrantyService
{
    public interface IWarrantyService
    {
        public Task<ResultModel> CreateWarranty(string? token, WarrantyCreateModel warrantyCreate);
        public Task<ResultModel> UpdateWarranty(string? token, WarrantyUpdateModel warrantyUpdate);
    }
}
