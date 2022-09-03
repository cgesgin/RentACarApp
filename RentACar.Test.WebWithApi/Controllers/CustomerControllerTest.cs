using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using RentACar.Core.DTOs;
using RentACar.Core.Models;
using RentACar.WebWithApi.Controllers;
using RentACar.WebWithApi.Service;

namespace RentACar.Test.WebWithApi.Controllers
{
    public class CustomerControllerTest
    {
        private readonly Mock<IApiService> _mock;
        private readonly CostumersController _costumersController;

        public CustomerControllerTest()
        {
            _mock = new Mock<IApiService>();
            _costumersController = new CostumersController(_mock.Object);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnView()
        {
            var result = await _costumersController.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnCostumerList()
        {
            _mock.Setup(x => x.GetAllAsync<CostumerDto>("Costumers")).ReturnsAsync(GetCostumerList);
            var result = await _costumersController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var list = Assert.IsAssignableFrom<List<CostumerDto>>(viewResult.Model);
            Assert.Equal<int>(3, list.Count());
        }

        [Fact]
        public void Save_ActionExecutes_ReturnView()
        {
            var result = _costumersController.Save();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void SavePost_InValidModelState_ReturnView()
        {
            _costumersController.ModelState.AddModelError("Email", "");
            var result = await _costumersController.Save(null);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void SavePost_ValidModelState_RedirectToIndex()
        {
            var result = await _costumersController.Save(GetCostumerList().First());
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async void SavePost_ValidModelState_SaveMethodExecute()
        {
            CostumerDto newCostumer = null;
            string url = null;

            _mock.Setup(x => x.SaveAsync<CostumerDto>(It.IsAny<string>(), It.IsAny<CostumerDto>())).Callback<string, CostumerDto>(
            (u, x) =>
            {
                newCostumer = x;
                url = u;
            });
            var result = await _costumersController.Save(GetCostumerList().First());
            _mock.Verify(x => x.SaveAsync<CostumerDto>("Costumers", It.IsAny<CostumerDto>()), Times.Once);
            Assert.Equal(GetCostumerList().First().Id, newCostumer.Id);
            Assert.Equal("Costumers", url);
        }

        [Fact]
        public async void SavePost_InValidModelState_NeverSaveMethodExecute()
        {
            _costumersController.ModelState.AddModelError("Email", "");
            var result = await _costumersController.Save(GetCostumerList().First());
            _mock.Verify(x => x.SaveAsync<CostumerDto>("Costumers", It.IsAny<CostumerDto>()), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        public async void Update_DataIsNull_ReturnIndex(int id)
        {
            CostumerDto costumerDto = null;
            _mock.Setup(x => x.GetByIdAsync<CostumerDto>($"Costumers/{id}")).ReturnsAsync(costumerDto);
            var result = await _costumersController.Update(id);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Theory]
        [InlineData(3)]
        public async void Update_DataIsNotNull_ReturnView(int id)
        {
            var costumer = GetCostumerList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync<CostumerDto>($"Costumers/{id}")).ReturnsAsync(costumer);
            var result = await _costumersController.Update(id);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CostumerDto>(viewResult.Model);
            Assert.Equal<int>(costumer.Id, model.Id);
        }

        [Fact]
        public async void UpdatePost_InValidExecutes_ReturnView()
        {
            CostumerDto costumer = GetCostumerList().First();
            _costumersController.ModelState.AddModelError("Email", "Email is Requerid");
            var result = await _costumersController.Update(costumer);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CostumerDto>(viewResult.Model);
            Assert.Equal<int>(costumer.Id, model.Id);
        }

        [Fact]
        public async void UpdatePost_ValidExecute_ReturnIndex()
        {
            CostumerDto costumer = GetCostumerList().First();
            var result = await _costumersController.Update(costumer);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async void UpdatePost_ValidExecute_UpdateMethodExecute()
        {
            var costumer = GetCostumerList().First();
            _mock.Setup(x => x.UpdateAsync<CostumerDto>("Costumers", costumer));
            await _costumersController.Update(costumer);
            _mock.Verify(x => x.UpdateAsync<CostumerDto>("Costumers", It.IsAny<CostumerDto>()), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async void Remove_ActionExecute_ReturnIndex(int id)
        {
            var result = await _costumersController.Remove(id);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_RemoveMethodExecute(int id)
        {
            var costumer = GetCostumerList().First(x => x.Id == id);
            var result = _mock.Setup(x => x.RemoveAsync($"Costumers/{It.IsAny<int>()}"));
            await _costumersController.Remove(id); 
            _mock.Verify(x => x.RemoveAsync($"Costumers/{costumer.Id}"), Times.Once);
        }

        private List<CostumerDto> GetCostumerList()
        {
            List<CostumerDto> list = new List<CostumerDto>()
            {
                new CostumerDto {Id=1,Name="John",LastName="Doe",Email="john@doe.com",Phone="555-555-5555" ,LisanceNo="02102100" },
                new CostumerDto {Id=2,Name="Jane",LastName="Doe",Email="jane@doe.com",Phone="555-555-4444" ,LisanceNo="02102101" },
                new CostumerDto {Id=3,Name="Hall",LastName="Doe",Email="hale@doe.com",Phone="555-555-3333" ,LisanceNo="02102102" }
            };
            return list;
        }
    }
}
