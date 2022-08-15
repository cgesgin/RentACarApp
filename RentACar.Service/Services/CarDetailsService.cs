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
    public class CarDetailsService : Service<CarDetails>,ICarDetailsService
    {
        private readonly ICarDetailsRepository _carDetailsRepository;
        private readonly IMapper _mapper;
        public CarDetailsService(IGenericRepository<CarDetails> repository, IUnitOfWork unitOfWork, IMapper mapper, ICarDetailsRepository carDetailsRepository) : base(repository, unitOfWork)
        {
            _mapper = mapper;
            _carDetailsRepository = carDetailsRepository;
        }
    }
}
