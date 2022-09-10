using Microsoft.AspNetCore.Mvc;
using Moq;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Controllers;
using RentACar.WebWithApi.Service;

namespace RentACar.Test.WebWithApi.Controllers
{
    public class PaymentsControllerTest
    {
        private readonly Mock<IApiService> _mock;
        private readonly PaymentsController _paymentsController;

        public PaymentsControllerTest()
        {
            _mock = new Mock<IApiService>();
            _paymentsController = new PaymentsController(_mock.Object);
        }

        [Theory]
        [InlineData(3)]
        public async void Save_DataIsNotNull_ReturnView(int id)
        {
            var rentalDto = GetRentalList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync<RentalDto>($"Rentals/{id}")).ReturnsAsync(rentalDto);
            var result = await _paymentsController.Save(id);
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void SavePost_ValidExecute_SaveMethodExecute()
        {
            var rentalDto = GetRentalList().First();
            _mock.Setup(x => x.GetByIdAsync<RentalDto>($"Rentals/{rentalDto.Id}")).ReturnsAsync(rentalDto);
            var payment = GetPaymentList().First();
            _mock.Setup(x => x.SaveAsync<PaymentDto>("Payments", payment));
            await _paymentsController.Save(payment);
            _mock.Verify(x => x.SaveAsync<PaymentDto>("Payments", It.IsAny<PaymentDto>()), Times.Once);
        }

        [Fact]
        public async void SavePost_ValidExecute_ReturnIndex()
        {
            var rentalDto = GetRentalList().First();
            _mock.Setup(x => x.GetByIdAsync<RentalDto>($"Rentals/{rentalDto.Id}")).ReturnsAsync(rentalDto);

            var payment = GetPaymentList().First();
            _mock.Setup(x => x.SaveAsync<PaymentDto>("Payments", payment));
            var result = await _paymentsController.Save(payment);
            Assert.IsType<RedirectResult>(result);
        }

        private List<PaymentDto> GetPaymentList()
        {
            List<PaymentDto> list = new List<PaymentDto>()
                    {
                        new PaymentDto {Id=1,CardName="john Doe",CardNo=123456789,CVV=013,ExpriyDate=DateTime.Now,Amount=620,RentalId=1},
                        new PaymentDto {Id=2,CardName="Jane Doe",CardNo=123456789,CVV=013,ExpriyDate=DateTime.Now,Amount=620,RentalId=1},
                        new PaymentDto {Id=3,CardName="Hall Doe",CardNo=123456789,CVV=013,ExpriyDate=DateTime.Now,Amount=620,RentalId=1},
                    };
            return list;
        }

        private List<RentalDto> GetRentalList()
        {
            List<RentalDto> list = new List<RentalDto>()
                    {
                        new RentalDto {Id=1,CarId=1,CostumerId=1,DropStoreId=1,RentalAmount=200,TotalAmount=500,RentalDate=DateTime.Now,ReturnDate=DateTime.Now },
                        new RentalDto {Id=2,CarId=1,CostumerId=1,DropStoreId=1,RentalAmount=200,TotalAmount=500,RentalDate=DateTime.Now,ReturnDate=DateTime.Now },
                        new RentalDto {Id=3,CarId=1,CostumerId=1,DropStoreId=1,RentalAmount=200,TotalAmount=500,RentalDate=DateTime.Now,ReturnDate=DateTime.Now },
                    };
            return list;
        }
    }
}
