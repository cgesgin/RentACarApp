using Microsoft.AspNetCore.Mvc;
using Moq;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Controllers;
using RentACar.WebWithApi.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Test.WebWithApi.Controllers
{
    public class CitiesControllerTest
    {
        private readonly Mock<IApiService> _mock;
        private readonly CitiesController _citiesController;


        public CitiesControllerTest()
        {
            _mock = new Mock<IApiService>();
            _citiesController = new CitiesController(_mock.Object);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnView()
        {
            var result = await _citiesController.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void SavePost_ValidModelState_SaveMethodExecute()
        {
            CityDto cityDto = null;
            string url = null;

            _mock.Setup(x => x.SaveAsync<CityDto>(It.IsAny<string>(), It.IsAny<CityDto>())).Callback<string, CityDto>(
            (u, x) =>
            {
                cityDto = x;
                url = u;
            });
            var result = await _citiesController.Save(GetCityList().First());
            _mock.Verify(x => x.SaveAsync<CityDto>("Cities", It.IsAny<CityDto>()), Times.Once);
            Assert.Equal(GetCityList().First().Id, cityDto.Id);
            Assert.Equal("Cities", url);
        }

        [Fact]
        public async void SavePost_InValidModelState_NeverSaveMethodExecute()
        {
            _citiesController.ModelState.AddModelError("", "");
            var result = await _citiesController.Save(GetCityList().First());
            _mock.Verify(x => x.SaveAsync<CityDto>("Cities", It.IsAny<CityDto>()), Times.Never);
        }

        [Theory]
        [InlineData(3)]
        public async void Update_DataIsNotNull_ReturnView(int id)
        {
            var city = GetCityList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync<CityDto>($"Cities/{id}")).ReturnsAsync(city);
            var result = await _citiesController.Update(id);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CityDto>(viewResult.Model);
            Assert.Equal<int>(city.Id, model.Id);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnList()
        {
            _mock.Setup(x => x.GetAllAsync<CityDto>("Cities")).ReturnsAsync(GetCityList);
            var result = await _citiesController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var list = Assert.IsAssignableFrom<List<CityDto>>(viewResult.Model);
            Assert.Equal<int>(3, list.Count());
        }

        [Fact]
        public async void UpdatePost_InValidExecutes_ReturnView()
        {
            var city = GetCityList().First();
            _citiesController.ModelState.AddModelError("Name", "name is required");
            var result = await _citiesController.Update(city);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CityDto>(viewResult.Model);
            Assert.Equal<int>(city.Id, model.Id);
        }

        [Fact]
        public async void UpdatePost_ValidExecute_ReturnIndex()
        {
            var city = GetCityList().First();
            var result = await _citiesController.Update(city);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async void UpdatePost_ValidExecute_UpdateMethodExecute()
        {
            var city = GetCityList().First();
            _mock.Setup(x => x.UpdateAsync<CityDto>("Cities", city));
            await _citiesController.Update(city);
            _mock.Verify(x => x.UpdateAsync<CityDto>("Cities", It.IsAny<CityDto>()), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async void Remove_ActionExecute_ReturnIndex(int id)
        {
            var result = await _citiesController.Remove(id);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_RemoveMethodExecute(int id)
        {
            var city = GetCityList().First(x => x.Id == id);
            var result = _mock.Setup(x => x.RemoveAsync($"Cities/{It.IsAny<int>()}"));
            await _citiesController.Remove(id);
            _mock.Verify(x => x.RemoveAsync($"Cities/{city.Id}"), Times.Once);
        }

        private List<CityDto> GetCityList()
        {
            List<CityDto> list = new List<CityDto>()
            {
                new CityDto {Id=1,Name="Test-1" },
                new CityDto {Id=2,Name="Test-2" },
                new CityDto {Id=3,Name="Test-3" }
            };
            return list;
        }
    }
}
