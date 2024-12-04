using Data.Model.ResultModel;
using Data.Model.VoucherModel;
using Data.Repository.VoucherRepo;

namespace Bussiness.Services.VoucherService
{
    public interface IVoucherService
    {
        public Task<ResultModel> CreateVoucher(string? token, VoucherCreateModel voucherCreate);

        Task<ResultModel> UpdateVoucher(string? token, VoucherRequestModel voucherUpdate);
        public Task<ResultModel> DeleteVoucherAsync(string? token, string voucherId);

        public Task<ResultModel> ViewListVoucher(string? token, VoucherSearchModel voucherSearch);
        public Task<ResultModel> ViewListVoucherv2(string? token, VoucherSearchModel voucherSearch);
    }
}
