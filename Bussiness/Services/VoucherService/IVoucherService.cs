using Data.Model.ResultModel;
using Data.Model.VoucherModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.VoucherService
{
    public interface IVoucherService
    {
        public Task<ResultModel> CreateVoucher(String token, VoucherCreateModel voucherCreate);
    }
}
