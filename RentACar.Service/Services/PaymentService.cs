using AutoMapper;
using RentACar.Core.Models;
using RentACar.Core.Repositories;
using RentACar.Core.Services;
using RentACar.Core.UnitOfWorks;
using RentACar.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Service.Services
{
    public class PaymentService : Service<Payment>, IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RabbitMQPublisher _rabbitMQPublisher;
        public PaymentService(IGenericRepository<Payment> repository, IUnitOfWork unitOfWork, IPaymentRepository paymentRepository, IMapper mapper, RabbitMQPublisher rabbitMQPublisher) : base(repository, unitOfWork)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        public async new Task<Payment> AddAsync(Payment entity)
        {
            await _paymentRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            var data = await _paymentRepository.GetByIdPaymentWithRentalAsync(entity.RentalId);
            _rabbitMQPublisher.Publish(new CustomerMailSendedForRentalEvent()
            {
                Amount = data.TotalAmount,
                RentalDate = data.RentalDate,
                ReturnDate = data.ReturnDate,
                CostumerName = data.Costumer.Name,
                CostumerLastName = data.Costumer.LastName,
                CostumerEmail=data.Costumer.Email,
                CarModel = data.Car.Model.Name,
                CarBrand = data.Car.Model.Brand.Name,
                DropStore = data.DropStore.Name,
                RentalStores = data.Car.RentalStore.Name
            });
            return entity;
        }
    }
}
