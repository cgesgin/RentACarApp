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
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        public BrandRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<Brand> GetByIdBrandWithModelsAsync(int brandId)
        {
            return await _appDbContext.Brands.Include(x => x.Models).Where(x => x.Id == brandId).SingleOrDefaultAsync();
        }
    }
}
