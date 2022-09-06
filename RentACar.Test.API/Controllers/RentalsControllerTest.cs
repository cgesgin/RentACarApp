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
    public class RentalsControllerTest
    {
        private readonly Mock<IRentalService> _mock;
        private readonly RentalsController _rentalsController;
        private readonly IMapper _mapper;

        public RentalsControllerTest()
        {
            _mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new MapProfile())));
            _mock = new Mock<IRentalService>();
            _rentalsController = new RentalsController(_mapper, _mock.Object);
        }

        [Fact]
        public async void GetAll_ActionExecute_ReturnSuccess()
        {
            _mock.Setup(x => x.GetAllAsync()).ReturnsAsync(GetRentalList());
            var result = await _rentalsController.GetAll();
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, response.StatusCode);
            var responseValue = Assert.IsType<ResponseDto<List<RentalDto>>>(response.Value);
            Assert.Equal(3, responseValue.Data.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetById_ActionExecute_ReturnSuccess(int id)
        {
            var rental = GetRentalList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(rental);
            var result = await _rentalsController.GetById(id);
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<RentalDto>>(response.Value);
            Assert.IsType<RentalDto>(responseValue.Data);
            Assert.Equal(rental.Id, responseValue.Data.Id);
        }

        [Fact]
        public async void Update_ActionExecute_ReturnSuccess()
        {
            var rental = GetRentalList().First();
            var rentalDto = _mapper.Map<RentalDto>(rental);
            _mock.Setup(x => x.UpdateAsync(rental));
            var result = await _rentalsController.Update(rentalDto);
            _mock.Verify(x => x.UpdateAsync(It.IsAny<Rental>()), Times.Exactly(1));
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);

        }

        [Fact]
        public async void Save_ActionExecute_ReturnSuccess()
        {
            var rental = GetRentalList().Find(x => x.Id == 1);
            _mock.Setup(m => m.AddAsync(It.IsAny<Rental>())).ReturnsAsync((Rental newRental) => newRental);
            var result = await _rentalsController.Save(_mapper.Map<RentalDto>(rental));
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<RentalDto>>(response.Value);
            Assert.IsType<RentalDto>(responseValue.Data);
            Assert.Equal(rental.Id, responseValue.Data.Id);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_ReturnSuccess(int id)
        {
            var rental = GetRentalList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(rental);
            _mock.Setup(x => x.RemoveAsync(rental));
            var result = await _rentalsController.Remove(id);
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);
            _mock.Verify(x => x.RemoveAsync(rental), Times.Once);
        }

        private List<Rental> GetRentalList()
        {
            List<Rental> list = new List<Rental>()
                    {
                        new Rental {Id=1,CarId=1,CostumerId=1,DropStoreId=1,RentalAmount=200,TotalAmount=500,RentalDate=DateTime.Now,ReturnDate=DateTime.Now },
                        new Rental {Id=2,CarId=1,CostumerId=1,DropStoreId=1,RentalAmount=200,TotalAmount=500,RentalDate=DateTime.Now,ReturnDate=DateTime.Now },
                        new Rental {Id=3,CarId=1,CostumerId=1,DropStoreId=1,RentalAmount=200,TotalAmount=500,RentalDate=DateTime.Now,ReturnDate=DateTime.Now },
                    };
            return list;
        }

    }
}
