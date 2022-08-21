using RentACar.Core.Models;
using RentACar.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Repository.Repositories
{
    public class CarTypeRepository : GenericRepository<CarType>, ICarTypeRepository
    {
        public CarTypeRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
