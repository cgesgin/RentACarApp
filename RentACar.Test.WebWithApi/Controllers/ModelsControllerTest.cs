using Microsoft.AspNetCore.Mvc;
using Moq;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Controllers;
using RentACar.WebWithApi.Service;


namespace RentACar.Test.WebWithApi.Controllers
{
    public class ModelsControllerTest
    {
        private readonly Mock<IApiService> _mock;
        private readonly ModelsController _modelsController;


        public ModelsControllerTest()
        {
            _mock = new Mock<IApiService>();
            _modelsController = new ModelsController(_mock.Object);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnView()
        {
            var result = await _modelsController.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void SavePost_ValidModelState_SaveMethodExecute()
        {
            ModelDto ModelDto = null;
            string url = null;

            _mock.Setup(x => x.SaveAsync<ModelDto>(It.IsAny<string>(), It.IsAny<ModelDto>())).Callback<string, ModelDto>(
            (u, x) =>
            {
                ModelDto = x;
                url = u;
            });
            var result = await _modelsController.Save(GetModelList().First());
            _mock.Verify(x => x.SaveAsync<ModelDto>("Models", It.IsAny<ModelDto>()), Times.Once);
            Assert.Equal(GetModelList().First().Id, ModelDto.Id);
            Assert.Equal("Models", url);
        }

        [Fact]
        public async void SavePost_InValidModelState_NeverSaveMethodExecute()
        {
            _modelsController.ModelState.AddModelError("", "");
            _mock.Setup(x => x.GetAllAsync<BrandDto>("Brands")).ReturnsAsync(GetBrandList);
            var result = await _modelsController.Save(GetModelList().First());
            _mock.Verify(x => x.SaveAsync<ModelDto>("Models", It.IsAny<ModelDto>()), Times.Never);
        }

        [Theory]
        [InlineData(3)]
        public async void Update_DataIsNotNull_ReturnView(int id)
        {
            var models = GetModelList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetAllAsync<BrandDto>("Brands")).ReturnsAsync(GetBrandList);
            _mock.Setup(x => x.GetByIdAsync<ModelDto>($"Models/{id}")).ReturnsAsync(models);
            var result = await _modelsController.Update(id);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ModelDto>(viewResult.Model);
            Assert.Equal<int>(models.Id, model.Id);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnList()
        {
            _mock.Setup(x => x.GetAllAsync<ModelWithBrandDto>("Models/GetModelWithBrand")).ReturnsAsync(GetModelWithBrandList);
            var result = await _modelsController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var list = Assert.IsAssignableFrom<List<ModelWithBrandDto>>(viewResult.Model);
            Assert.Equal<int>(3, list.Count());
        }

        [Fact]
        public async void UpdatePost_InValidExecutes_ReturnView()
        {
            var models = GetModelList().First();
            _mock.Setup(x => x.GetAllAsync<BrandDto>("Brands")).ReturnsAsync(GetBrandList);
            _modelsController.ModelState.AddModelError("", "");
            var result = await _modelsController.Update(models);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ModelDto>(viewResult.Model);
            Assert.Equal<int>(models.Id, model.Id);
        }

        [Fact]
        public async void UpdatePost_ValidExecute_ReturnIndex()
        {
            var models = GetModelList().First();
            var result = await _modelsController.Update(models);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async void UpdatePost_ValidExecute_UpdateMethodExecute()
        {
            var models = GetModelList().First();
            _mock.Setup(x => x.UpdateAsync<ModelDto>("Models", models));
            await _modelsController.Update(models);
            _mock.Verify(x => x.UpdateAsync<ModelDto>("Models", It.IsAny<ModelDto>()), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async void Remove_ActionExecute_ReturnIndex(int id)
        {
            var result = await _modelsController.Remove(id);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_RemoveMethodExecute(int id)
        {
            var models = GetModelList().First(x => x.Id == id);
            var result = _mock.Setup(x => x.RemoveAsync($"Models/{It.IsAny<int>()}"));
            await _modelsController.Remove(id);
            _mock.Verify(x => x.RemoveAsync($"Models/{models.Id}"), Times.Once);
        }

        private List<ModelDto> GetModelList()
        {
            List<ModelDto> list = new List<ModelDto>()
            {
                new ModelDto {Id=1,Name="Test-1" },
                new ModelDto {Id=2,Name="Test-2" },
                new ModelDto {Id=3,Name="Test-3" }
            };
            return list;
        }
        private List<BrandDto> GetBrandList()
        {
            List<BrandDto> list = new List<BrandDto>()
            {
                new BrandDto {Id=1,Name="Test-1" },
                new BrandDto {Id=2,Name="Test-2" },
                new BrandDto {Id=3,Name="Test-3" }
            };
            return list;
        }
        private List<ModelWithBrandDto> GetModelWithBrandList()
        {
            var brand = new BrandDto { Id = 1, Name = "Test-1" };
            List<ModelWithBrandDto> list = new List<ModelWithBrandDto>()
            {
                new ModelWithBrandDto {Id=1,Name="Test-1" ,Brand=brand},
                new ModelWithBrandDto {Id=2,Name="Test-2" ,Brand=brand},
                new ModelWithBrandDto {Id=3,Name="Test-3" , Brand = brand}
            };
            return list;
        }
    }
}
