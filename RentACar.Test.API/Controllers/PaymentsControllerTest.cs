using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RentACar.API.Controllers;
using RentACar.Core.DTOs;
using RentACar.Core.Models;
using RentACar.Core.Services;
using RentACar.Service.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Test.API.Controllers
{
    public class PaymentsControllerTest
    {
        private readonly Mock<IPaymentService> _mock;
        private readonly PaymentsController _paymentsController;
        private readonly IMapper _mapper;

        public PaymentsControllerTest()
        {
            _mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new MapProfile())));
            _mock = new Mock<IPaymentService>();
            _paymentsController = new PaymentsController(_mapper, _mock.Object);
        }

        [Fact]
        public async void GetAll_ActionExecute_ReturnSuccess()
        {
            _mock.Setup(x => x.GetAllAsync()).ReturnsAsync(GetPaymentList());
            var result = await _paymentsController.GetAll();
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, response.StatusCode);
            var responseValue = Assert.IsType<ResponseDto<List<PaymentDto>>>(response.Value);
            Assert.Equal(3, responseValue.Data.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetById_ActionExecute_ReturnSuccess(int id)
        {
            var payment = GetPaymentList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(payment);
            var result = await _paymentsController.GetById(id);
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<PaymentDto>>(response.Value);
            Assert.IsType<PaymentDto>(responseValue.Data);
            Assert.Equal(payment.Id, responseValue.Data.Id);
        }


        [Fact]
        public async void Update_ActionExecute_ReturnSuccess()
        {
            var payment = GetPaymentList().First();
            var paymentDto = _mapper.Map<PaymentDto>(payment);
            _mock.Setup(x => x.UpdateAsync(payment));
            var result = await _paymentsController.Update(paymentDto);
            _mock.Verify(x => x.UpdateAsync(It.IsAny<Payment>()), Times.Exactly(1));
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);

        }

        [Fact]
        public async void Save_ActionExecute_ReturnSuccess()
        {
            var payment = GetPaymentList().Find(x => x.Id == 1);
            _mock.Setup(m => m.AddAsync(It.IsAny<Payment>())).ReturnsAsync((Payment newPayment) => newPayment);
            var result = await _paymentsController.Save(_mapper.Map<PaymentDto>(payment));
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<PaymentDto>>(response.Value);
            Assert.IsType<PaymentDto>(responseValue.Data);
            Assert.Equal(payment.Id, responseValue.Data.Id);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_ReturnSuccess(int id)
        {
            var payment = GetPaymentList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(payment);
            _mock.Setup(x => x.RemoveAsync(payment));
            var result = await _paymentsController.Remove(id);
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);
            _mock.Verify(x => x.RemoveAsync(payment), Times.Once);
        }

        private List<Payment> GetPaymentList()
        {
            List<Payment> list = new List<Payment>()
                    {
                        new Payment {Id=1,CardName="john Doe",CardNo=123456789,CVV=013,ExpriyDate=DateTime.Now,Amount=620,RentalId=1},
                        new Payment {Id=2,CardName="Jane Doe",CardNo=123456789,CVV=013,ExpriyDate=DateTime.Now,Amount=620,RentalId=1},
                        new Payment {Id=3,CardName="Hall Doe",CardNo=123456789,CVV=013,ExpriyDate=DateTime.Now,Amount=620,RentalId=1},
                    };
            return list;
        }
    }
}