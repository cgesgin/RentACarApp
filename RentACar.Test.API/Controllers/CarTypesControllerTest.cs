using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RentACar.API.Controllers;
using RentACar.Core.DTOs;
using RentACar.Core.Models;
using RentACar.Core.Services;
using RentACar.Service.Mapping;

namespace RentACar.Test.API.Controllers
{
    public class CarTypesControllerTest
    {
        private readonly Mock<ICarTypeService> _mock;
        private readonly CarTypesController _carTypesController;
        private readonly IMapper _mapper;

        public CarTypesControllerTest()
        {
            _mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new MapProfile())));
            _mock = new Mock<ICarTypeService>();
            _carTypesController = new CarTypesController(_mapper, _mock.Object);
        }

        [Fact]
        public async void GetAll_ActionExecute_ReturnSuccess()
        {
            _mock.Setup(x => x.GetAllAsync()).ReturnsAsync(GetCarTypeList());
            var result = await _carTypesController.GetAll();
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, response.StatusCode);
            var responseValue = Assert.IsType<ResponseDto<List<CarTypeDto>>>(response.Value);
            Assert.Equal(3, responseValue.Data.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetById_ActionExecute_ReturnSuccess(int id)
        {
            var carType = GetCarTypeList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(carType);
            var result = await _carTypesController.GetById(id);
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<CarTypeDto>>(response.Value);
            Assert.IsType<CarTypeDto>(responseValue.Data);
            Assert.Equal(carType.Id, responseValue.Data.Id);
        }


        [Fact]
        public async void Update_ActionExecute_ReturnSuccess()
        {
            var carType = GetCarTypeList().First();
            var carTypeDto = _mapper.Map<CarTypeDto>(carType);
            _mock.Setup(x => x.UpdateAsync(carType));
            var result = await _carTypesController.Update(carTypeDto);
            _mock.Verify(x => x.UpdateAsync(It.IsAny<CarType>()), Times.Exactly(1));
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);

        }



        [Fact]
        public async void Save_ActionExecute_ReturnSuccess()
        {
            var carType = GetCarTypeList().Find(x => x.Id == 1);
            _mock.Setup(m => m.AddAsync(It.IsAny<CarType>())).ReturnsAsync((CarType newCarType) => newCarType);
            var result = await _carTypesController.Save(_mapper.Map<CarTypeDto>(carType));
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<CarTypeDto>>(response.Value);
            Assert.IsType<CarTypeDto>(responseValue.Data);
            Assert.Equal(carType.Id, responseValue.Data.Id);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_ReturnSuccess(int id)
        {
            var carType = GetCarTypeList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(carType);
            _mock.Setup(x => x.RemoveAsync(carType));
            var result = await _carTypesController.Remove(id);
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);
            _mock.Verify(x => x.RemoveAsync(carType), Times.Once);
        }

        private List<CarType> GetCarTypeList()
        {
            List<CarType> list = new List<CarType>()
            {
                new CarType {Id=1,Name="Sedan" },
                new CarType {Id=2,Name="Hatchback" },
                new CarType {Id=3,Name="Cuv" },
            };
            return list;
        }
    }
}