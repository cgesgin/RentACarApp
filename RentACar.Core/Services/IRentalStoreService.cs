using RentACar.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.Services
{
    public interface IRentalStoreService : IService<RentalStore>
    {
        Task<List<RentalStore>> GetRentalStoreWithAddressAsync();
        Task<RentalStore> GetByIdRentalStoreWithAddressAsync(int id);
    }
}
