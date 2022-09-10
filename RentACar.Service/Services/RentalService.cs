using AutoMapper;
using RentACar.Core.Models;
using RentACar.Core.Repositories;
using RentACar.Core.Services;
using RentACar.Core.UnitOfWorks;


namespace RentACar.Service.Services
{
    public class RentalService : Service<Rental> , IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;

        public RentalService(IGenericRepository<Rental> repository, IUnitOfWork unitOfWork, IRentalRepository rentalRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
        }

        public Task<List<Rental>> GetByUserIdWithCarAndCostumerAsync(string userId)
        {
            return _rentalRepository.GetByUserIdWithCarAndCostumerAsync(userId);
        }

        public Task<List<Rental>> GetRentalWithCarAndCostumerAsync()
        {
            return _rentalRepository.GetRentalWithCarAndCostumerAsync();
        }
    }
}
