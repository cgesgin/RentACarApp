
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
    public class RentalStoreService : Service<RentalStore>, IRentalStoreService
    {
        private readonly IRentalStoreRepository _rentalStoreRepository;
        private readonly IMapper _mapper;
        public RentalStoreService(IGenericRepository<RentalStore> repository, IUnitOfWork unitOfWork, IRentalStoreRepository rentalStoreRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _rentalStoreRepository = rentalStoreRepository;
            _mapper = mapper;
        }

        public Task<RentalStore> GetByIdRentalStoreWithAddressAsync(int id)
        {
            return _rentalStoreRepository.GetByIdRentalStoreWithAddressAsync(id);
        }

        public Task<List<RentalStore>> GetRentalStoreWithAddressAsync()
        {
            return _rentalStoreRepository.GetRentalStoreWithAddressAsync();
        }
    }
}
