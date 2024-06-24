using Data.Entities;
using Data.Model.GemModel;
using Data.Model.ResultModel;
using Data.Model.VoucherModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.VoucherRepo
{
    public interface IVoucherRepo
    {
        public Task<Voucher> CreateVoucherAsync(Voucher voucher);
        public Task<Voucher> UpdateVoucherAsync(Voucher voucher);
        public Task<Voucher> GetVoucherByIdAsync(string voucherId);
        public Task<Voucher> DeleteVoucherAsync(Voucher voucher);
        IQueryable<Voucher> GetVoucherQuery();
    }
}
