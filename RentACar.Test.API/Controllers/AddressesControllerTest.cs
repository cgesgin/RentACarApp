using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RentACar.API.Controllers;
using RentACar.Core.DTOs;
using RentACar.Core.Models;
using RentACar.Core.Services;
using RentACar.Service.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Test.API.Controllers
{
    public class AddressesControllerTest
    {
        private readonly Mock<IAddressService> _mock;
        private readonly AddressesController _addressesController;
        private readonly IMapper _mapper;

        public AddressesControllerTest()
        {
            _mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new MapProfile())));
            _mock = new Mock<IAddressService>();
            _addressesController = new AddressesController(_mapper, _mock.Object);
        }


        [Fact]
        public async void GetAll_ActionExecute_ReturnSuccess()
        {
            _mock.Setup(x => x.GetAllAsync()).ReturnsAsync(GetAddressesList());
            var result = await _addressesController.GetAll();
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, response.StatusCode);
            var responseValue = Assert.IsType<ResponseDto<List<AddressDto>>>(response.Value);
            Assert.Equal(3, responseValue.Data.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetById_ActionExecute_ReturnSuccess(int id)
        {
            var address = GetAddressesList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(address);
            var result = await _addressesController.GetById(id);
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<AddressDto>>(response.Value);
            Assert.IsType<AddressDto>(responseValue.Data);
            Assert.Equal(address.Id, responseValue.Data.Id);
        }

        [Fact]
        public async void Update_ActionExecute_ReturnSuccess()
        {
            var address = GetAddressesList().First();

            var addressDto = _mapper.Map<AddressDto>(address);
            _mock.Setup(x => x.UpdateAsync(address));
            var result = await _addressesController.Update(addressDto);
            _mock.Verify(x => x.UpdateAsync(It.IsAny<Address>()), Times.Once);
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);

        }

        [Fact]
        public async void Save_ActionExecute_ReturnSuccess()
        {
            var address = GetAddressesList().Find(x => x.Id == 1);
            _mock.Setup(m => m.AddAsync(It.IsAny<Address>())).ReturnsAsync((Address newAddress) => newAddress);
            var result = await _addressesController.Save(_mapper.Map<AddressDto>(address));
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<AddressDto>>(response.Value);
            Assert.IsType<AddressDto>(responseValue.Data);
            Assert.Equal(address.Id, responseValue.Data.Id);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_ReturnSuccess(int id)
        {

            var address = GetAddressesList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(address);
            _mock.Setup(x => x.RemoveAsync(address));
            var result = await _addressesController.Remove(id);
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);
            _mock.Verify(x => x.RemoveAsync(address), Times.Once);
        }



        private List<Address> GetAddressesList()
        {
            List<Address> list = new List<Address>()
            {
                new Address {Id=1,Street="street-1",Neighbourhood="neighbourhood",description="home",Zipcode=123,DistrictId=1 },
                new Address {Id=2,Street="street-2",Neighbourhood="neighbourhood",description="office",Zipcode=123,DistrictId=2 },
                new Address {Id=3,Street="street-3",Neighbourhood="neighbourhood",description="home-office",Zipcode=123,DistrictId=3 }
            };
            return list;
        }
    }
}
