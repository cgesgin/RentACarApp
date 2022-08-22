using AutoMapper;
using RentACar.Core.DTOs;
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
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public AddressService(IGenericRepository<Address> repository, IUnitOfWork unitOfWork, IAddressRepository addressRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<List<Address>> GetAddressWithDistrictAsync()
        {
            return await _addressRepository.GetAddressWithDistrictAsync();
        }
    }
}
