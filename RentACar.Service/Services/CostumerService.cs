using AutoMapper;
using RentACar.Core.Models;
using RentACar.Core.Repositories;
using RentACar.Core.Services;
using RentACar.Core.UnitOfWorks;


namespace RentACar.Service.Services
{
    public class CostumerService : Service<Costumer>,ICostumerService
    {
        private readonly ICostumerRepository _costumerRepository;
        private readonly IMapper _mapper;
        public CostumerService(IGenericRepository<Costumer> repository, IUnitOfWork unitOfWork, ICostumerRepository costumerRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _costumerRepository = costumerRepository;
            _mapper = mapper;
        }
    }
}
