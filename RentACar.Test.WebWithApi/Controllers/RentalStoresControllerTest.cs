using Microsoft.AspNetCore.Mvc;
using Moq;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Controllers;
using RentACar.WebWithApi.Service;

namespace RentACar.Test.WebWithApi.Controllers
{
    public class RentalStoresControllerTest
    {
        private readonly Mock<IApiService> _mock;
        private readonly RentalStoresController _rentalStoresController;

        public RentalStoresControllerTest()
        {
            _mock = new Mock<IApiService>();
            _rentalStoresController = new RentalStoresController(_mock.Object);
        }


        [Fact]
        public async void Index_ActionExecutes_ReturnView()
        {
            var result = await _rentalStoresController.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnList()
        {
            _mock.Setup(x => x.GetAllAsync<RentalStoreWithAddressDto>("RentalStores/GetRentalStoreWithAddress")).ReturnsAsync(GetRentalStoreWithAddressList);
            var result = await _rentalStoresController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var list = Assert.IsAssignableFrom<List<RentalStoreWithAddressDto>>(viewResult.Model);
            Assert.Equal<int>(3, list.Count());
        }

        [Fact]
        public async void SavePost_ValidModelState_SaveMethodExecute()
        {
            RentalStoreDto rentalStoreDto = null;
            string url = null;

            _mock.Setup(x => x.SaveAsync<AddressDto>(It.IsAny<string>(), It.IsAny<AddressDto>())).ReturnsAsync(GetAddressList().First());
            _mock.Setup(x => x.SaveAsync<RentalStoreDto>(It.IsAny<string>(), It.IsAny<RentalStoreDto>())).Callback<string, RentalStoreDto>(
            (u, x) =>
            {
                rentalStoreDto = x;
                url = u;
            });
            var result = await _rentalStoresController.Save(GetRentalStoreWithAddressList().First());
            _mock.Verify(x => x.SaveAsync<RentalStoreDto>("RentalStores", It.IsAny<RentalStoreDto>()), Times.Once);
            Assert.Equal(GetRentalStoreList().First().Id, rentalStoreDto.Id);
            Assert.Equal("RentalStores", url);
        }

        [Fact]
        public async void SavePost_InValidModelState_NeverSaveMethodExecute()
        {
            RentalStoreWithAddressDto data = null;
            _rentalStoresController.ModelState.AddModelError("", "");
            _mock.Setup(x => x.GetAllAsync<DistrictDto>("Districts")).ReturnsAsync(GetDistrictList);
            var result = await _rentalStoresController.Save(data);
            _mock.Verify(x => x.SaveAsync<RentalStoreDto>("RentalStores", It.IsAny<RentalStoreDto>()), Times.Never);

        }

        [Theory]
        [InlineData(3)]
        public async void Update_DataIsNotNull_ReturnView(int id)
        {
            var rentalStore = GetRentalStoreWithAddressList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetAllAsync<DistrictDto>("Districts")).ReturnsAsync(GetDistrictList);
            _mock.Setup(x => x.GetByIdAsync<RentalStoreWithAddressDto>($"RentalStores/GetByIdRentalStoreWithAddress/{id}")).ReturnsAsync(rentalStore);

            var result = await _rentalStoresController.Update(id);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RentalStoreWithAddressDto>(viewResult.Model);
            Assert.Equal<int>(rentalStore.Id, model.Id);
        }



        [Fact]
        public async void UpdatePost_InValidExecutes_ReturnView()
        {
            var rentalStore = GetRentalStoreWithAddressList().First();
            _rentalStoresController.ModelState.AddModelError("", "");
            _mock.Setup(x => x.GetAllAsync<DistrictDto>("Districts")).ReturnsAsync(GetDistrictList);
            var result = await _rentalStoresController.Update(rentalStore);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<RentalStoreWithAddressDto>(viewResult.Model);
            Assert.Equal<int>(rentalStore.Id, model.Id);
        }

        [Fact]
        public async void UpdatePost_ValidExecute_ReturnIndex()
        {
            var rentalStore = GetRentalStoreWithAddressList().First();
            var result = await _rentalStoresController.Update(rentalStore);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }
        
        [Fact]
        public async void UpdatePost_ValidExecute_UpdateMethodExecute()
        {
            var rentalStore = GetRentalStoreWithAddressList().First();
            _mock.Setup(x => x.UpdateAsync<RentalStoreDto>("RentalStores", rentalStore));
            await _rentalStoresController.Update(rentalStore);
            _mock.Verify(x => x.UpdateAsync<RentalStoreDto>("RentalStores", It.IsAny<RentalStoreDto>()), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async void Remove_ActionExecute_ReturnIndex(int id)
        {
            var rentalStore = GetRentalStoreList().First(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync<RentalStoreDto>($"RentalStores/{rentalStore.Id}")).ReturnsAsync(rentalStore);
            var result = await _rentalStoresController.Remove(id);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_RemoveMethodExecute(int id)
        {
            var rentalStore = GetRentalStoreList().First(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync<RentalStoreDto>($"RentalStores/{rentalStore.Id}")).ReturnsAsync(rentalStore);
            _mock.Setup(x => x.RemoveAsync($"RentalStores/{It.IsAny<int>()}"));
            await _rentalStoresController.Remove(id);
            _mock.Verify(x => x.RemoveAsync($"RentalStores/{rentalStore.Id}"), Times.Once);
        }

        private List<RentalStoreWithAddressDto> GetRentalStoreWithAddressList()
        {
            List<RentalStoreWithAddressDto> list = new List<RentalStoreWithAddressDto>()
                 {
                     new RentalStoreWithAddressDto {Id=1,Name="Store Name",phone="555555",Email="email@email.com",AddressId=1 },
                     new RentalStoreWithAddressDto {Id=2,Name="Store Name",phone="555555",Email="email@email.com",AddressId=1 },
                     new RentalStoreWithAddressDto{Id=3,Name="Store Name",phone="555555",Email="email@email.com",AddressId=1 }
                 };
            return list;
        }
        private List<RentalStoreDto> GetRentalStoreList()
        {
            List<RentalStoreDto> list = new List<RentalStoreDto>()
            {
                new RentalStoreDto {Id=1,Name="Store Name",phone="555555",Email="email@email.com",AddressId=1 },
                new RentalStoreDto {Id=2,Name="Store Name",phone="555555",Email="email@email.com",AddressId=1 },
                new RentalStoreDto {Id=3,Name="Store Name",phone="555555",Email="email@email.com",AddressId=1 },
            };
            return list;
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
    }
}
