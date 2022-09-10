using Microsoft.AspNetCore.Mvc;
using Moq;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Controllers;
using RentACar.WebWithApi.Service;

namespace RentACar.Test.WebWithApi.Controllers
{
    public class CarDetailsControllerTest
    {

        private readonly Mock<IApiService> _mock;
        private readonly CarDetailsController _carDetailsController;

        public CarDetailsControllerTest()
        {
            _mock = new Mock<IApiService>();
            _carDetailsController = new CarDetailsController(_mock.Object);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnView()
        {
            var result = await _carDetailsController.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void SavePost_ValidModelState_SaveMethodExecute()
        {
            CarDetailsDto carDetailsDto = null;
            string url = null;

            _mock.Setup(x => x.SaveAsync<CarDetailsDto>(It.IsAny<string>(), It.IsAny<CarDetailsDto>())).Callback<string, CarDetailsDto>(
            (u, x) =>
            {
                carDetailsDto = x;
                url = u;
            });
            var result = await _carDetailsController.Save(GetCarDetailList().First());
            _mock.Verify(x => x.SaveAsync<CarDetailsDto>("CarDetails", It.IsAny<CarDetailsDto>()), Times.Once);
            Assert.Equal(GetCarDetailList().First().Id, carDetailsDto.Id);
            Assert.Equal("CarDetails", url);
        }

        [Fact]
        public async void SavePost_InValidModelState_NeverSaveMethodExecute()
        {
            _carDetailsController.ModelState.AddModelError("", "");
            var result = await _carDetailsController.Save(GetCarDetailList().First());
            _mock.Verify(x => x.SaveAsync<CarDetailsDto>("CarDetails", It.IsAny<CarDetailsDto>()), Times.Never);
        }

        [Theory]
        [InlineData(3)]
        public async void Update_DataIsNotNull_ReturnView(int id)
        {
            var carDetails = GetCarDetailList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync<CarDetailsDto>($"CarDetails/{id}")).ReturnsAsync(carDetails);
            var result = await _carDetailsController.Update(id);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CarDetailsDto>(viewResult.Model);
            Assert.Equal<int>(carDetails.Id, model.Id);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnList()
        {
            _mock.Setup(x => x.GetAllAsync<CarDetailsDto>("CarDetails")).ReturnsAsync(GetCarDetailList);
            var result = await _carDetailsController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var list = Assert.IsAssignableFrom<List<CarDetailsDto>>(viewResult.Model);
            Assert.Equal<int>(3, list.Count());
        }

        [Fact]
        public async void UpdatePost_InValidExecutes_ReturnView()
        {
            var carDetails = GetCarDetailList().First();
            _carDetailsController.ModelState.AddModelError("", "");
            var result = await _carDetailsController.Update(carDetails);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CarDetailsDto>(viewResult.Model);
            Assert.Equal<int>(carDetails.Id, model.Id);
        }

        [Fact]
        public async void UpdatePost_ValidExecute_ReturnIndex()
        {
            var carDetails = GetCarDetailList().First();
            var result = await _carDetailsController.Update(carDetails);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async void UpdatePost_ValidExecute_UpdateMethodExecute()
        {
            var carDetails = GetCarDetailList().First();
            _mock.Setup(x => x.UpdateAsync<CarDetailsDto>("CarDetails", carDetails));
            await _carDetailsController.Update(carDetails);
            _mock.Verify(x => x.UpdateAsync<CarDetailsDto>("CarDetails", It.IsAny<CarDetailsDto>()), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async void Remove_ActionExecute_ReturnIndex(int id)
        {
            var result = await _carDetailsController.Remove(id);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_RemoveMethodExecute(int id)
        {
            var carDetails = GetCarDetailList().First(x => x.Id == id);
            var result = _mock.Setup(x => x.RemoveAsync($"CarDetails/{It.IsAny<int>()}"));
            await _carDetailsController.Remove(id);
            _mock.Verify(x => x.RemoveAsync($"CarDetails/{carDetails.Id}"), Times.Once);
        }

        private List<CarDetailsDto> GetCarDetailList()
        {
            List<CarDetailsDto> list = new List<CarDetailsDto>()
            {
                new CarDetailsDto {Id=1,Color="test",CarYear=2001 },
                new CarDetailsDto {Id=2,Color="test",CarYear=2001 },
                new CarDetailsDto {Id=3,Color="test",CarYear=2001 },
            };
            return list;
        }
    }
}
