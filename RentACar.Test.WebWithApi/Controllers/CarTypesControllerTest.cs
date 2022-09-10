using Microsoft.AspNetCore.Mvc;
using Moq;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Controllers;
using RentACar.WebWithApi.Service;

namespace RentACar.Test.WebWithApi.Controllers
{
    public class CarTypesControllerTest
    {
        private readonly Mock<IApiService> _mock;
        private readonly CarTypesController _carTypesController;


        public CarTypesControllerTest()
        {
            _mock = new Mock<IApiService>();
            _carTypesController = new CarTypesController(_mock.Object);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnView()
        {
            var result = await _carTypesController.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void SavePost_ValidModelState_SaveMethodExecute()
        {
            CarTypeDto CarTypeDto = null;
            string url = null;

            _mock.Setup(x => x.SaveAsync<CarTypeDto>(It.IsAny<string>(), It.IsAny<CarTypeDto>())).Callback<string, CarTypeDto>(
            (u, x) =>
            {
                CarTypeDto = x;
                url = u;
            });
            var result = await _carTypesController.Save(GetCarTypeList().First());
            _mock.Verify(x => x.SaveAsync<CarTypeDto>("CarTypes", It.IsAny<CarTypeDto>()), Times.Once);
            Assert.Equal(GetCarTypeList().First().Id, CarTypeDto.Id);
            Assert.Equal("CarTypes", url);
        }

        [Fact]
        public async void SavePost_InValidModelState_NeverSaveMethodExecute()
        {
            _carTypesController.ModelState.AddModelError("", "");
            var result = await _carTypesController.Save(GetCarTypeList().First());
            _mock.Verify(x => x.SaveAsync<CarTypeDto>("CarTypes", It.IsAny<CarTypeDto>()), Times.Never);
        }

        [Theory]
        [InlineData(3)]
        public async void Update_DataIsNotNull_ReturnView(int id)
        {
            var carType = GetCarTypeList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync<CarTypeDto>($"CarTypes/{id}")).ReturnsAsync(carType);
            var result = await _carTypesController.Update(id);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CarTypeDto>(viewResult.Model);
            Assert.Equal<int>(carType.Id, model.Id);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnList()
        {
            _mock.Setup(x => x.GetAllAsync<CarTypeDto>("CarTypes")).ReturnsAsync(GetCarTypeList);
            var result = await _carTypesController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var list = Assert.IsAssignableFrom<List<CarTypeDto>>(viewResult.Model);
            Assert.Equal<int>(3, list.Count());
        }

        [Fact]
        public async void UpdatePost_InValidExecutes_ReturnView()
        {
            var carType = GetCarTypeList().First();
            _carTypesController.ModelState.AddModelError("", "");
            var result = await _carTypesController.Update(carType);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CarTypeDto>(viewResult.Model);
            Assert.Equal<int>(carType.Id, model.Id);
        }

        [Fact]
        public async void UpdatePost_ValidExecute_ReturnIndex()
        {
            var carType = GetCarTypeList().First();
            var result = await _carTypesController.Update(carType);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async void UpdatePost_ValidExecute_UpdateMethodExecute()
        {
            var carType = GetCarTypeList().First();
            _mock.Setup(x => x.UpdateAsync<CarTypeDto>("CarTypes", carType));
            await _carTypesController.Update(carType);
            _mock.Verify(x => x.UpdateAsync<CarTypeDto>("CarTypes", It.IsAny<CarTypeDto>()), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async void Remove_ActionExecute_ReturnIndex(int id)
        {
            var result = await _carTypesController.Remove(id);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_RemoveMethodExecute(int id)
        {
            var carType = GetCarTypeList().First(x => x.Id == id);
            var result = _mock.Setup(x => x.RemoveAsync($"CarTypes/{It.IsAny<int>()}"));
            await _carTypesController.Remove(id);
            _mock.Verify(x => x.RemoveAsync($"CarTypes/{carType.Id}"), Times.Once);
        }

        private List<CarTypeDto> GetCarTypeList()
        {
            List<CarTypeDto> list = new List<CarTypeDto>()
            {
                new CarTypeDto {Id=1,Name="Test-1" },
                new CarTypeDto {Id=2,Name="Test-2" },
                new CarTypeDto {Id=3,Name="Test-3" }
            };
            return list;
        }
    }
}
