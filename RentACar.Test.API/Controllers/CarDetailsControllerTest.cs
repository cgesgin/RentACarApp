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
    public class CarDetailsControllerTest
    {

        private readonly Mock<ICarDetailsService> _mock;
        private readonly CarDetailsController _carDetailsController;
        private readonly IMapper _mapper;

        public CarDetailsControllerTest()
        {
            _mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new MapProfile())));
            _mock = new Mock<ICarDetailsService>();
            _carDetailsController = new CarDetailsController(_mapper, _mock.Object);
        }

        [Fact]
        public async void GetAll_ActionExecute_ReturnSuccess()
        {
            _mock.Setup(x => x.GetAllAsync()).ReturnsAsync(GetCarDetailsList());
            var result = await _carDetailsController.GetAll();
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, response.StatusCode);
            var responseValue = Assert.IsType<ResponseDto<List<CarDetailsDto>>>(response.Value);
            Assert.Equal(3, responseValue.Data.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetById_ActionExecute_ReturnSuccess(int id)
        {
            var carDetails = GetCarDetailsList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(carDetails);
            var result = await _carDetailsController.GetById(id);
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<CarDetailsDto>>(response.Value);
            Assert.IsType<CarDetailsDto>(responseValue.Data);
            Assert.Equal(carDetails.Id, responseValue.Data.Id);
        }


        [Fact]
        public async void Update_ActionExecute_ReturnSuccess()
        {
            var carDetails = GetCarDetailsList().First();
            var carDetailsDto = _mapper.Map<CarDetailsDto>(carDetails);
            _mock.Setup(x => x.UpdateAsync(carDetails));
            var result = await _carDetailsController.Update(carDetailsDto);
            _mock.Verify(x => x.UpdateAsync(It.IsAny<CarDetails>()), Times.Exactly(1));
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);

        }



        [Fact]
        public async void Save_ActionExecute_ReturnSuccess()
        {
            var carDetails = GetCarDetailsList().Find(x => x.Id == 1);
            _mock.Setup(m => m.AddAsync(It.IsAny<CarDetails>())).ReturnsAsync((CarDetails newCarDetails) => newCarDetails);
            var result = await _carDetailsController.Save(_mapper.Map<CarDetailsDto>(carDetails));
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<CarDetailsDto>>(response.Value);
            Assert.IsType<CarDetailsDto>(responseValue.Data);
            Assert.Equal(carDetails.Id, responseValue.Data.Id);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_ReturnSuccess(int id)
        {
            var carDetails = GetCarDetailsList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(carDetails);
            _mock.Setup(x => x.RemoveAsync(carDetails));
            var result = await _carDetailsController.Remove(id);
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);
            _mock.Verify(x => x.RemoveAsync(carDetails), Times.Once);
        }


        private List<CarDetails> GetCarDetailsList()
        {
            List<CarDetails> list = new List<CarDetails>()
            {
                new CarDetails {Id=1,CarYear=2000,Color="Green" },
                new CarDetails {Id=2,CarYear=2010,Color="Red" },
                new CarDetails {Id=3,CarYear=2020,Color="Grey" }
            };
            return list;
        }

    }
}
