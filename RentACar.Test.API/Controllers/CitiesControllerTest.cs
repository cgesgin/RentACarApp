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
    public class CitiesControllerTest
    {
        private readonly Mock<ICityService> _mock;
        private readonly CitiesController _citiesController;
        private readonly IMapper _mapper;

        public CitiesControllerTest()
        {
            _mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new MapProfile())));
            _mock = new Mock<ICityService>();
            _citiesController = new CitiesController(_mapper, _mock.Object);
        }
        
        [Fact]
        public async void GetAll_ActionExecute_ReturnSuccess()
        {
            _mock.Setup(x => x.GetAllAsync()).ReturnsAsync(GetCityList());
            var result = await _citiesController.GetAll();
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, response.StatusCode);
            var responseValue = Assert.IsType<ResponseDto<List<CityDto>>>(response.Value);
            Assert.Equal(3, responseValue.Data.Count);
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetById_ActionExecute_ReturnSuccess(int id)
        {
            var city = GetCityList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(city);
            var result = await _citiesController.GetById(id);
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<CityDto>>(response.Value);
            Assert.IsType<CityDto>(responseValue.Data);
            Assert.Equal(city.Id, responseValue.Data.Id);
        }


        [Fact]
        public async void Update_ActionExecute_ReturnSuccess()
        {
            var city = GetCityList().First();
            var cityDto = _mapper.Map<CityDto>(city);
            _mock.Setup(x => x.UpdateAsync(city));
            var result = await _citiesController.Update(cityDto);
            _mock.Verify(x => x.UpdateAsync(It.IsAny<City>()), Times.Exactly(1));
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);
        }

        [Fact]
        public async void Save_ActionExecute_ReturnSuccess()
        {
            var city = GetCityList().Find(x => x.Id == 1);
            _mock.Setup(m => m.AddAsync(It.IsAny<City>())).ReturnsAsync((City newCity) => newCity);
            var result = await _citiesController.Save(_mapper.Map<CityDto>(city));
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<CityDto>>(response.Value);
            Assert.IsType<CityDto>(responseValue.Data);
            Assert.Equal(city.Id, responseValue.Data.Id);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_ReturnSuccess(int id)
        {
            var city = GetCityList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(city);
            _mock.Setup(x => x.RemoveAsync(city));
            var result = await _citiesController.Remove(id);
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);
            _mock.Verify(x => x.RemoveAsync(city), Times.Once);
        }

        private List<City> GetCityList()
        {
            List<City> list = new List<City>()
            {
                new City {Id=1,Name="City-1" },
                new City {Id=2,Name="City-2" },
                new City {Id=3,Name="City-3" },
            };
            return list;
        }
    }
}