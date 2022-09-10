using Microsoft.AspNetCore.Mvc;
using Moq;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Controllers;
using RentACar.WebWithApi.Service;

namespace RentACar.Test.WebWithApi.Controllers
{
    public class AddressesControllerTest
    {
        private readonly Mock<IApiService> _mock;
        private readonly AddressesController _addressesController;

        public AddressesControllerTest()
        {
            _mock = new Mock<IApiService>();
            _addressesController = new AddressesController(_mock.Object);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnView()
        {
            var result = await _addressesController.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void SavePost_ValidModelState_SaveMethodExecute()
        {
            AddressDto addressDto = null;
            string url = null;

            _mock.Setup(x => x.SaveAsync<AddressDto>(It.IsAny<string>(), It.IsAny<AddressDto>())).Callback<string, AddressDto>(
            (u, x) =>
            {
                addressDto = x;
                url = u;
            });
            var result = await _addressesController.Save(GetAddressList().First());
            _mock.Verify(x => x.SaveAsync<AddressDto>("Addresses", It.IsAny<AddressDto>()), Times.Once);
            Assert.Equal(GetAddressList().First().Id, addressDto.Id);
            Assert.Equal("Addresses", url);
        }

        [Fact]
        public async void SavePost_InValidModelState_NeverSaveMethodExecute()
        {
            
            _mock.Setup(x => x.GetAllAsync<DistrictDto>("Districts")).ReturnsAsync(GetDistrictList);
            _addressesController.ModelState.AddModelError("", "");
            var result = await _addressesController.Save(GetAddressList().First());
            _mock.Verify(x => x.SaveAsync<AddressDto>("Addresses", It.IsAny<AddressDto>()), Times.Never);
        }

        [Theory]
        [InlineData(3)]
        public async void Update_DataIsNotNull_ReturnView(int id)
        {
            var address = GetAddressList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetAllAsync<DistrictDto>("Districts")).ReturnsAsync(GetDistrictList);
            _mock.Setup(x => x.GetByIdAsync<AddressDto>($"Addresses/{id}")).ReturnsAsync(address);
            var result = await _addressesController.Update(id);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AddressDto>(viewResult.Model);
            Assert.Equal<int>(address.Id, model.Id);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnList()
        {
            _mock.Setup(x => x.GetAllAsync<AddressWithDistrictDto>("Addresses/GetAddressWithDistrict")).ReturnsAsync(GetAddressWithDistrictList);
            var result = await _addressesController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var list = Assert.IsAssignableFrom<List<AddressWithDistrictDto>>(viewResult.Model);
            Assert.Equal<int>(3, list.Count());
        }

        [Fact]
        public async void UpdatePost_InValidExecutes_ReturnView()
        {
            var address = GetAddressList().First();
            _mock.Setup(x => x.GetAllAsync<DistrictDto>("Districts")).ReturnsAsync(GetDistrictList);
            _addressesController.ModelState.AddModelError("", "");
            var result = await _addressesController.Update(address);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<AddressDto>(viewResult.Model);
            Assert.Equal<int>(address.Id, model.Id);
        }

        [Fact]
        public async void UpdatePost_ValidExecute_ReturnIndex()
        {
            var address = GetAddressList().First();
            var result = await _addressesController.Update(address);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async void UpdatePost_ValidExecute_UpdateMethodExecute()
        {
            var address = GetAddressList().First();
            _mock.Setup(x => x.UpdateAsync<AddressDto>("Addresses", address));
            await _addressesController.Update(address);
            _mock.Verify(x => x.UpdateAsync<AddressDto>("Addresses", It.IsAny<AddressDto>()), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async void Remove_ActionExecute_ReturnIndex(int id)
        {
            var result = await _addressesController.Remove(id);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_RemoveMethodExecute(int id)
        {
            var address = GetAddressList().First(x => x.Id == id);
            var result = _mock.Setup(x => x.RemoveAsync($"Addresses/{It.IsAny<int>()}"));
            await _addressesController.Remove(id);
            _mock.Verify(x => x.RemoveAsync($"Addresses/{address.Id}"), Times.Once);
        }

        private List<AddressDto> GetAddressList()
        {
            List<AddressDto> list = new List<AddressDto>()
            {
                new AddressDto {Id=1,description="Test",Neighbourhood="test",Street="test",Zipcode=123,DistrictId=1},
                new AddressDto {Id=2,description="Test",Neighbourhood="test",Street="test",Zipcode=123,DistrictId=1},
                new AddressDto {Id=3,description="Test",Neighbourhood="test",Street="test",Zipcode=123,DistrictId=1},
            };
            return list;
        }
       
        private List<AddressWithDistrictDto> GetAddressWithDistrictList()
        {
            var district = new DistrictDto { Id = 1, Name = "test", CityId = 1 };
            List<AddressWithDistrictDto> list = new List<AddressWithDistrictDto>()
            {
                new AddressWithDistrictDto {Id=1,description="Test",Neighbourhood="test",Street="test",Zipcode=123,DistrictId=1,District=district},
                new AddressWithDistrictDto {Id=2,description="Test",Neighbourhood="test",Street="test",Zipcode=123,DistrictId=1,District=district},
                new AddressWithDistrictDto {Id=3,description="Test",Neighbourhood="test",Street="test",Zipcode=123,DistrictId=1,District=district},
            };
            return list;
        }

        private List<DistrictDto> GetDistrictList()
        {
            List<DistrictDto> list = new List<DistrictDto>()
            { 
                new DistrictDto {Id=1,Name="test",CityId=1 }
            };
            return list;
        }
    }
}
