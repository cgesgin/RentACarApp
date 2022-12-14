using RentACar.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.Repositories
{
    public interface IRentalRepository : IGenericRepository<Rental>
    {
        Task<List<Rental>> GetByUserIdWithCarAndCostumerAsync(string userId);
        Task<List<Rental>> GetRentalWithCarAndCostumerAsync();
    }
}
