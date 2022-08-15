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
    public class AddressService : Service<Address>, IAddressService
    {
        private readonly IModelRepository _modelRepository;
        private readonly IMapper _mapper;

        public AddressService(IGenericRepository<Address> repository, IUnitOfWork unitOfWork, IModelRepository modelRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _modelRepository = modelRepository;
            _mapper = mapper;
        }
    }
}
