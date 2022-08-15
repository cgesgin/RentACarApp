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
    public class DistrictService : Service<District> , IDistrictService
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly IMapper _mapper;

        public DistrictService(IGenericRepository<District> repository, IUnitOfWork unitOfWork, IDistrictRepository districtRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _districtRepository = districtRepository;
            _mapper = mapper;
        }
    }
}
