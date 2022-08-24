using RentACar.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.Repositories
{
    public interface IRentalStoreRepository : IGenericRepository<RentalStore>
    {
        Task<List<RentalStore>> GetRentalStoreWithAddressAsync();
        Task<RentalStore> GetByIdRentalStoreWithAddressAsync(int id);
    }
}
