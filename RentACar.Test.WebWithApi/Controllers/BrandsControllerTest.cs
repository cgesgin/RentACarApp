using Microsoft.AspNetCore.Mvc;
using Moq;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Controllers;
using RentACar.WebWithApi.Service;

namespace RentACar.Test.WebWithApi.Controllers
{
    public class BrandsControllerTest
    {
        private readonly Mock<IApiService> _mock;
        private readonly BrandsController _brandsController;


        public BrandsControllerTest()
        {
            _mock = new Mock<IApiService>();
            _brandsController = new BrandsController(_mock.Object);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnView()
        {
            var result = await _brandsController.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void SavePost_ValidModelState_SaveMethodExecute()
        {
            BrandDto brandDto = null;
            string url = null;

            _mock.Setup(x => x.SaveAsync<BrandDto>(It.IsAny<string>(), It.IsAny<BrandDto>())).Callback<string, BrandDto>(
            (u, x) =>
            {
                brandDto = x;
                url = u;
            });
            var result = await _brandsController.Save(GetBrandList().First());
            _mock.Verify(x => x.SaveAsync<BrandDto>("Brands", It.IsAny<BrandDto>()), Times.Once);
            Assert.Equal(GetBrandList().First().Id, brandDto.Id);
            Assert.Equal("Brands", url);
        }

        [Fact]
        public async void SavePost_InValidModelState_NeverSaveMethodExecute()
        {
            _brandsController.ModelState.AddModelError("", "");
            var result = await _brandsController.Save(GetBrandList().First());
            _mock.Verify(x => x.SaveAsync<BrandDto>("Brands", It.IsAny<BrandDto>()), Times.Never);
        }

        [Theory]
        [InlineData(3)]
        public async void Update_DataIsNotNull_ReturnView(int id)
        {
            var brand = GetBrandList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync<BrandDto>($"Brands/{id}")).ReturnsAsync(brand);
            var result = await _brandsController.Update(id);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<BrandDto>(viewResult.Model);
            Assert.Equal<int>(brand.Id, model.Id);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnList()
        {
            _mock.Setup(x => x.GetAllAsync<BrandDto>("Brands")).ReturnsAsync(GetBrandList);
            var result = await _brandsController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var list = Assert.IsAssignableFrom<List<BrandDto>>(viewResult.Model);
            Assert.Equal<int>(3, list.Count());
        }

        [Fact]
        public async void UpdatePost_InValidExecutes_ReturnView()
        {
            var brand = GetBrandList().First();
            _brandsController.ModelState.AddModelError("", "");
            var result = await _brandsController.Update(brand);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<BrandDto>(viewResult.Model);
            Assert.Equal<int>(brand.Id, model.Id);
        }

        [Fact]
        public async void UpdatePost_ValidExecute_ReturnIndex()
        {
            var brand = GetBrandList().First();
            var result = await _brandsController.Update(brand);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async void UpdatePost_ValidExecute_UpdateMethodExecute()
        {
            var brand = GetBrandList().First();
            _mock.Setup(x => x.UpdateAsync<BrandDto>("Brands", brand));
            await _brandsController.Update(brand);
            _mock.Verify(x => x.UpdateAsync<BrandDto>("Brands", It.IsAny<BrandDto>()), Times.Once);
        } 

        [Theory]
        [InlineData(1)]
        public async void Remove_ActionExecute_ReturnIndex(int id)
        {
            var result = await _brandsController.Remove(id);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }
       
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_RemoveMethodExecute(int id)
        {
            var brand = GetBrandList().First(x => x.Id == id);
            var result = _mock.Setup(x => x.RemoveAsync($"Brands/{It.IsAny<int>()}"));
            await _brandsController.Remove(id);
            _mock.Verify(x => x.RemoveAsync($"Brands/{brand.Id}"), Times.Once);
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
    }
}
