using Data.Entities;
using Data.Repository.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.UserRepo
{
    public interface IUserRepo :IRepository<User>
    {
        public Task<User> GetByUsernameAsync(string username);

        public Task<List<User>> GetAllUser();

        public Task<User> GetByIdAsync(string id);


    }
}
