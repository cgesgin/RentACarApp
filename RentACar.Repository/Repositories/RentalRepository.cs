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
    public class RentalRepository : GenericRepository<Rental>, IRentalRepository
    {
        public RentalRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<List<Rental>> GetByUserIdWithCarAndCostumerAsync(string userId)
        {
            var data = await _appDbContext.Rentals
                .Include(x => x.DropStore)
                .Include(x => x.Costumer)
                .Include(x => x.Car)
                .ThenInclude(x => x.Model)
                .ThenInclude(x => x.Brand)
                .OrderBy(x => x.Id)
                .Where(x => x.Costumer.UserId == userId).ToListAsync();
            data.ForEach(x =>
                x.Car = _appDbContext.Cars.Include(x => x.RentalStore).First(p => p.Id == x.CarId)
            );
            return data;
        }

        public async Task<List<Rental>> GetRentalWithCarAndCostumerAsync()
        {
            var data = await _appDbContext.Rentals
               .Include(x => x.DropStore)
               .Include(x => x.Costumer)
               .Include(x => x.Car)
               .ThenInclude(x => x.Model)
               .ThenInclude(x => x.Brand)
               .OrderByDescending(x => x.Id)
               .ToListAsync();
            data.ForEach(x =>
                x.Car = _appDbContext.Cars.Include(x => x.RentalStore).FirstOrDefault(p => p.Id == x.CarId)
            );
            return data;
        }
    }
}
