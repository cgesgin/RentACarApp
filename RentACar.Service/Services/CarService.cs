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
    public class CarService : Service<Car>,ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;

        public CarService(IGenericRepository<Car> repository, IUnitOfWork unitOfWork, ICarRepository carRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }
    }
}
