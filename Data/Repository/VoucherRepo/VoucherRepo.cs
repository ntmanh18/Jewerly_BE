using Data.Entities;
using Data.Model.ResultModel;
using Data.Model.VoucherModel;
using Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.VoucherRepo
{
    public class VoucherRepo : Repository<Voucher>, IVoucherRepo
    {
        private readonly JewerlyV6Context _context;
        public VoucherRepo(JewerlyV6Context context) : base(context)
        {
            _context = context;
        }
        public async Task<Voucher> CreateVoucherAsync(Voucher voucher)
        {
            var lastVoucher = await _context.Vouchers
                .OrderByDescending(g => g.VoucherId)
                .FirstOrDefaultAsync();

            int newIdNumber = 1;
            if (lastVoucher != null)
            {
                int lastIdNumber;
                if (int.TryParse(lastVoucher.VoucherId.Substring(1), out lastIdNumber))
                {
                    newIdNumber = lastIdNumber + 1;
                }
            }
            voucher.VoucherId = $"V{newIdNumber:000}";

            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync();
            return voucher;
        }

        public async Task<Voucher> DeleteVoucherAsync(Voucher voucher)
        {
            _context.Vouchers.Remove(voucher);
            await _context.SaveChangesAsync();
            return voucher;
        }

        public async Task<Voucher> GetVoucherByIdAsync(string voucherId) => _context.Vouchers.FirstOrDefault(g => g.VoucherId == voucherId);

        public IQueryable<Voucher> GetVoucherQuery() => _context.Vouchers.Include(v => v.CustomerCustomer).AsQueryable();

        public async Task<Voucher> UpdateVoucherAsync(Voucher voucherUpdate)
        {
            try
            {
                _context.Update(voucherUpdate);
                await _context.SaveChangesAsync();
                return voucherUpdate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
