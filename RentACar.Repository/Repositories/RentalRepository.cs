using RentACar.Core.Models;
using RentACar.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Repository.Repositories
{
    public class RentalRepository : GenericRepository<Rental>,IRentalRepository
    {
        public RentalRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
