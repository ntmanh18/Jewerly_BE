using Data.Entities;
using Data.Repository.GenericRepo;

namespace Data.Repository.ProductBillRepo
{
    public interface IProductBillRepo : IRepository<ProductBill>
    {
        Task<ProductBill> GetUniqueProductBill(string billId, string productId);
        Task<List<ProductBill>> GetProducByBillId(string billId);
    }
}
