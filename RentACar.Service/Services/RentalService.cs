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
    public class RentalService : Service<Rental> , IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;
        public RentalService(IGenericRepository<Rental> repository, IUnitOfWork unitOfWork, IRentalRepository rentalRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
        }
    }
}
