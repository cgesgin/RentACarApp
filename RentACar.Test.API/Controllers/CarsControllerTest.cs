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
    public class CarsControllerTest
    {
        private readonly Mock<ICarService> _mock;
        private readonly CarsController _carsController;
        private readonly IMapper _mapper;

        public CarsControllerTest()
        {
            _mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new MapProfile())));
            _mock = new Mock<ICarService>();
            _carsController = new CarsController(_mapper, _mock.Object);
        }

        [Fact]
        public async void GetAll_ActionExecute_ReturnSuccess()
        {
            _mock.Setup(x => x.GetAllAsync()).ReturnsAsync(GetCarList());
            var result = await _carsController.GetAll();
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, response.StatusCode);
            var responseValue = Assert.IsType<ResponseDto<List<CarDto>>>(response.Value);
            Assert.Equal(3, responseValue.Data.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetById_ActionExecute_ReturnSuccess(int id)
        {
            var car = GetCarList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(car);
            var result = await _carsController.GetById(id);
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<CarDto>>(response.Value);
            Assert.IsType<CarDto>(responseValue.Data);
            Assert.Equal(car.Id, responseValue.Data.Id);
        }


        [Fact]
        public async void Update_ActionExecute_ReturnSuccess()
        {
            var car = GetCarList().First();
            var carDto = _mapper.Map<CarDto>(car);
            _mock.Setup(x => x.UpdateAsync(car));
            var result = await _carsController.Update(carDto);
            _mock.Verify(x => x.UpdateAsync(It.IsAny<Car>()), Times.Exactly(1));
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);

        }



        [Fact]
        public async void Save_ActionExecute_ReturnSuccess()
        {
            var car = GetCarList().Find(x => x.Id == 1);
            _mock.Setup(m => m.AddAsync(It.IsAny<Car>())).ReturnsAsync((Car newCar) => newCar);
            var result = await _carsController.Save(_mapper.Map<CarDto>(car));
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<CarDto>>(response.Value);
            Assert.IsType<CarDto>(responseValue.Data);
            Assert.Equal(car.Id, responseValue.Data.Id);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_ReturnSuccess(int id)
        {
            var car = GetCarList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(car);
            _mock.Setup(x => x.RemoveAsync(car));
            var result = await _carsController.Remove(id);
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);
            _mock.Verify(x => x.RemoveAsync(car), Times.Once);
        }

        private List<Car> GetCarList()
        {
            List<Car> list = new List<Car>()
            {
                new Car {Id=1,SeatCapacity=4,GearType="Manuel",FuelType="Gas",Price=500,CarTypeId=1,ModelId=1,CarDetailsId=1 },
                new Car {Id=2,SeatCapacity=4,GearType="Manuel",FuelType="Gas",Price=500,CarTypeId=1,ModelId=1,CarDetailsId=1 },
                new Car {Id=3,SeatCapacity=4,GearType="Manuel",FuelType="Gas",Price=500,CarTypeId=1,ModelId=1,CarDetailsId=1 },
            };
            return list;
        }

    }
}
