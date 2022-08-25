﻿using RentACar.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Core.Repositories
{
    public interface ICarRepository : IGenericRepository<Car>
    {
        Task<List<Car>> GetCarWithFeatureAsync();
        Task<Car> GetByIdCarWithFeatureAsync(int id);
    }
}
