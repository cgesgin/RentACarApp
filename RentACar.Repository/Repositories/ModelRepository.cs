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
    public class ModelRepository : GenericRepository<Model>, IModelRepository
    {
        public ModelRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<List<Model>> GetModelsWithBrandAsync()
        {
            return await _appDbContext.Models.Include(x=>x.Brand).ToListAsync();
        }
    }
}
