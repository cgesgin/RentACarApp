using RentACar.Core.Models;
using RentACar.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Repository.Repositories
{
    public class CarRepository : GenericRepository<Car>, ICarRepository
    {
        public CarRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
