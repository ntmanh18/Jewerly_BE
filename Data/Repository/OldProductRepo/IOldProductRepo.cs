using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.OldProductRepo
{
    public interface IOldProductRepo
    {
        Task<IEnumerable<OldProduct>> GetAllAsync();
        Task<OldProduct> GetByIdAsync(string id);
        Task<IEnumerable<OldProduct>> GetByProductIdAsync(string productId);
        Task AddAsync(OldProduct oldProduct);

    }
}
