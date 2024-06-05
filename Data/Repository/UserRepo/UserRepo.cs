using Data.Entities;
using Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.UserRepo
{
    public class UserRepo : Repository<User>, IUserRepo
    {
        private readonly JewerlyV6Context _context;
        public UserRepo(JewerlyV6Context context) : base(context)
        {
            _context = context;
        }


        public async Task<List<User>> GetAllUser()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<List<User>> GetAllUserQuery()
        {
            return await _context.Users.Select(
                u => new User
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    Password = u.Password,
                    Role = u.Role,
                    FullName = u.FullName,
                    DoB = u.DoB,
                    Phone = u.Phone,
                    Address = u.Address,
                    Status = u.Status

                }).AsQueryable().ToListAsync();   
                
                ;
                }
 

      
        

        public async Task<User> GetByIdAsync(string id)
        {
        return  _context.Users.FirstOrDefault(c => c.UserId == id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == username);
        }

    }
}
