using Data.Entities;
using Data.Repository.GenericRepo;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.GoldRepo
{
    public interface IGoldRepo : IRepository<Gold>
    {
        public Task<IEnumerable<Gold>> GetGolds();
        public Task<Gold> GetGoldById(string id);
        public Task CreateGold(Gold goldFilter);
        public Task<User> GetByIdAsync(string id);
        public Task<Gold> UpdateGold(Gold goldUpdate);
    }
}
