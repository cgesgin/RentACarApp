using AutoMapper;
using RentACar.Core.Models;
using RentACar.Core.Repositories;
using RentACar.Core.Services;
using RentACar.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Service.Services
{
    public class CityService : Service<City>,ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;
        public CityService(IGenericRepository<City> repository, IUnitOfWork unitOfWork, ICityRepository cityRepository) : base(repository, unitOfWork)
        {
            _cityRepository = cityRepository;
        }
    }
}
