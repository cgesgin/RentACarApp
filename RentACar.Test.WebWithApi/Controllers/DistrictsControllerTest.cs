using Microsoft.AspNetCore.Mvc;
using Moq;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Controllers;
using RentACar.WebWithApi.Service;

namespace RentACar.Test.WebWithApi.Controllers
{
    public class DistrictsControllerTest
    {
        private readonly Mock<IApiService> _mock;
        private readonly DistrictsController _districtsController;


        public DistrictsControllerTest()
        {
            _mock = new Mock<IApiService>();
            _districtsController = new DistrictsController(_mock.Object);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnView()
        {
            var result = await _districtsController.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void SavePost_ValidModelState_SaveMethodExecute()
        {
            DistrictDto DistrictDto = null;
            string url = null;

            _mock.Setup(x => x.SaveAsync<DistrictDto>(It.IsAny<string>(), It.IsAny<DistrictDto>())).Callback<string, DistrictDto>(
            (u, x) =>
            {
                DistrictDto = x;
                url = u;
            });
            var result = await _districtsController.Save(GetDistrictList().First());
            _mock.Verify(x => x.SaveAsync<DistrictDto>("Districts", It.IsAny<DistrictDto>()), Times.Once);
            Assert.Equal(GetDistrictList().First().Id, DistrictDto.Id);
            Assert.Equal("Districts", url);
        }

        [Fact]
        public async void SavePost_InValidModelState_NeverSaveMethodExecute()
        {
            _districtsController.ModelState.AddModelError("", "");
            _mock.Setup(x => x.GetAllAsync<CityDto>("Cities")).ReturnsAsync(GetCityList);
            var result = await _districtsController.Save(GetDistrictList().First());
            _mock.Verify(x => x.SaveAsync<DistrictDto>("Districts", It.IsAny<DistrictDto>()), Times.Never);
        }

        [Theory]
        [InlineData(3)]
        public async void Update_DataIsNotNull_ReturnView(int id)
        {
            var district = GetDistrictList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetAllAsync<CityDto>("Cities")).ReturnsAsync(GetCityList);
            _mock.Setup(x => x.GetByIdAsync<DistrictDto>($"Districts/{id}")).ReturnsAsync(district);
            var result = await _districtsController.Update(id);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<DistrictDto>(viewResult.Model);
            Assert.Equal<int>(district.Id, model.Id);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnList()
        {
            _mock.Setup(x => x.GetAllAsync<DistrictDto>("Districts")).ReturnsAsync(GetDistrictList);
            var result = await _districtsController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var list = Assert.IsAssignableFrom<List<DistrictDto>>(viewResult.Model);
            Assert.Equal<int>(3, list.Count());
        }

        [Fact]
        public async void UpdatePost_InValidExecutes_ReturnView()
        {
            var district = GetDistrictList().First();
            _mock.Setup(x => x.GetAllAsync<CityDto>("Cities")).ReturnsAsync(GetCityList);
            _districtsController.ModelState.AddModelError("", "");
            var result = await _districtsController.Update(district);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<DistrictDto>(viewResult.Model);
            Assert.Equal<int>(district.Id, model.Id);
        }

        [Fact]
        public async void UpdatePost_ValidExecute_ReturnIndex()
        {
            var district = GetDistrictList().First();
            var result = await _districtsController.Update(district);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async void UpdatePost_ValidExecute_UpdateMethodExecute()
        {
            var district = GetDistrictList().First();
            _mock.Setup(x => x.UpdateAsync<DistrictDto>("Districts", district));
            await _districtsController.Update(district);
            _mock.Verify(x => x.UpdateAsync<DistrictDto>("Districts", It.IsAny<DistrictDto>()), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async void Remove_ActionExecute_ReturnIndex(int id)
        {
            var result = await _districtsController.Remove(id);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_RemoveMethodExecute(int id)
        {
            var district = GetDistrictList().First(x => x.Id == id);
            var result = _mock.Setup(x => x.RemoveAsync($"Districts/{It.IsAny<int>()}"));
            await _districtsController.Remove(id);
            _mock.Verify(x => x.RemoveAsync($"Districts/{district.Id}"), Times.Once);
        }

        private List<DistrictDto> GetDistrictList()
        {
            List<DistrictDto> list = new List<DistrictDto>()
            {
                new DistrictDto {Id=1,Name="Test-1" ,CityId=1 },
                new DistrictDto {Id=2,Name="Test-2" ,CityId=1 },
                new DistrictDto {Id=3,Name="Test-3" ,CityId=1 }
            };
            return list;
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