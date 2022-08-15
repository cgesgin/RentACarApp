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
    public class CarTypeService : Service<CarType>,ICarTypeService
    {
        private readonly ICarTypeRepository _carTypeRepository;
        private readonly IMapper _mapper;
        public CarTypeService(IGenericRepository<CarType> repository, IUnitOfWork unitOfWork, ICarTypeRepository carTypeRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _carTypeRepository = carTypeRepository;
            _mapper = mapper;
        }
    }
}
