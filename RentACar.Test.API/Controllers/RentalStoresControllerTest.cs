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
    public class RentalStoresControllerTest
    {
        private readonly Mock<IRentalStoreService> _mock;
        private readonly RentalStoresController _rentalStoresController;
        private readonly IMapper _mapper;

        public RentalStoresControllerTest()
        {
            _mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new MapProfile())));
            _mock = new Mock<IRentalStoreService>();
            _rentalStoresController = new RentalStoresController(_mapper, _mock.Object);
        }


        [Fact]
        public async void GetAll_ActionExecute_ReturnSuccess()
        {
            _mock.Setup(x => x.GetAllAsync()).ReturnsAsync(GetRentalStoreList());
            var result = await _rentalStoresController.GetAll();
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, response.StatusCode);
            var responseValue = Assert.IsType<ResponseDto<List<RentalStoreDto>>>(response.Value);
            Assert.Equal(3, responseValue.Data.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetById_ActionExecute_ReturnSuccess(int id)
        {
            var rentalStore = GetRentalStoreList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(rentalStore);
            var result = await _rentalStoresController.GetById(id);
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<RentalStoreDto>>(response.Value);
            Assert.IsType<RentalStoreDto>(responseValue.Data);
            Assert.Equal(rentalStore.Id, responseValue.Data.Id);
        }

        [Fact]
        public async void Update_ActionExecute_ReturnSuccess()
        {
            var rentalStore = GetRentalStoreList().First();

            var rentalStoreDto = _mapper.Map<RentalStoreDto>(rentalStore);
            _mock.Setup(x => x.UpdateAsync(rentalStore));
            var result = await _rentalStoresController.Update(rentalStoreDto);
            _mock.Verify(x => x.UpdateAsync(It.IsAny<RentalStore>()), Times.Once);
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);

        }

        [Fact]
        public async void Save_ActionExecute_ReturnSuccess()
        {
            var rentalStore = GetRentalStoreList().Find(x => x.Id == 1);
            _mock.Setup(m => m.AddAsync(It.IsAny<RentalStore>())).ReturnsAsync((RentalStore store) => store);
            var result = await _rentalStoresController.Save(_mapper.Map<RentalStoreDto>(rentalStore));
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<RentalStoreDto>>(response.Value);
            Assert.IsType<RentalStoreDto>(responseValue.Data);
            Assert.Equal(rentalStore.Id, responseValue.Data.Id);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_ReturnSuccess(int id)
        {

            var rentalStore = GetRentalStoreList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(rentalStore);
            _mock.Setup(x => x.RemoveAsync(rentalStore));
            var result = await _rentalStoresController.Remove(id);
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);
            _mock.Verify(x => x.RemoveAsync(rentalStore), Times.Once);
        }

        private List<RentalStore> GetRentalStoreList()
        {
            List<RentalStore> list = new List<RentalStore>()
            {
                new RentalStore {Id=1,Name="Store Name",phone="555555",Email="email@email.com",AddressId=1 },
                new RentalStore {Id=2,Name="Store Name",phone="555555",Email="email@email.com",AddressId=1 },
                new RentalStore {Id=3,Name="Store Name",phone="555555",Email="email@email.com",AddressId=1 },
            };
            return list;
        }
    }
}
