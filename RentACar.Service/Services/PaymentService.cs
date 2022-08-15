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
    public class PaymentService : Service<Payment>,IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        public PaymentService(IGenericRepository<Payment> repository, IUnitOfWork unitOfWork, IPaymentRepository paymentRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }
    }
}
