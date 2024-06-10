using Data.Model.ResultModel;
using Data.Model.VoucherModel;

namespace Bussiness.Services.VoucherService
{
    public interface IVoucherService
    {
        public Task<ResultModel> CreateVoucher(String token, VoucherCreateModel voucherCreate);

        Task<ResultModel> UpdateVoucher(string? token, VoucherRequestModel voucherUpdate);
    }
}
