using Data.Entities;
using Data.Repository.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.BillRepo
{
    public class BillRepo : Repository<Bill>, IBillRepo
    {
        private readonly JewerlyV6Context _context;
        public BillRepo(JewerlyV6Context context) : base(context) 
        {
            _context = context;
        }
    }
}
