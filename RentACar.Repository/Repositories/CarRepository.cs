using Microsoft.EntityFrameworkCore;
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

        public Task<Car> GetByIdCarWithFeatureAsync(int id)
        {
            return _appDbContext.Cars
             .Include(x => x.RentalStore)
             .Include(x => x.CarDetails)
             .Include(x => x.CarType)
             .Include(x => x.Model)
             .ThenInclude(x => x.Brand)
             .FirstOrDefaultAsync(x=>x.Id == id);
        }

        public Task<List<Car>> GetCarWithFeatureAsync()
        {
            var cars =_appDbContext.Cars
                .Include(x => x.RentalStore)
                .Include(x => x.CarDetails)
                .Include(x => x.CarType)
                .Include(x => x.Model)
                .ThenInclude(x => x.Brand)
                .OrderBy(x => x.Id)
                .Where(x => x.Status.ToUpper() == "RENT")
                .ToListAsync();
            return cars;
        }
    }
}
