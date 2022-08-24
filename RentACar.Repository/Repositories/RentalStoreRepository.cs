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
    public class RentalStoreRepository : GenericRepository<RentalStore>,IRentalStoreRepository
    {
        public RentalStoreRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public Task<RentalStore> GetByIdRentalStoreWithAddressAsync(int id)
        {
            return _appDbContext.RentalStores
                .Include(x => x.Address)
                .ThenInclude(x => x.District)
                .FirstAsync(x=>x.Id==id);
        }

        public Task<List<RentalStore>> GetRentalStoreWithAddressAsync()
        {
            return _appDbContext.RentalStores
                .Include(x=>x.Address)
                .ThenInclude(x=>x.District)
                .OrderBy(x=>x.Id)
                .ToListAsync();
        }
    }
}
