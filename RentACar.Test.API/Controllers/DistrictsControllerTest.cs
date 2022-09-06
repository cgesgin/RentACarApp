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
    public class DistrictsControllerTest
    {
        private readonly Mock<IDistrictService> _mock;
        private readonly DistrictsController _districtsController;
        private readonly IMapper _mapper;

        public DistrictsControllerTest()
        {
            _mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new MapProfile())));
            _mock = new Mock<IDistrictService>();
            _districtsController = new DistrictsController(_mapper, _mock.Object);
        }

        [Fact]
        public async void GetAll_ActionExecute_ReturnSuccess()
        {
            _mock.Setup(x => x.GetAllAsync()).ReturnsAsync(GetDistrictList());
            var result = await _districtsController.GetAll();
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, response.StatusCode);
            var responseValue = Assert.IsType<ResponseDto<List<DistrictDto>>>(response.Value);
            Assert.Equal(3, responseValue.Data.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetById_ActionExecute_ReturnSuccess(int id)
        {
            var district = GetDistrictList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(district);
            var result = await _districtsController.GetById(id);
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<DistrictDto>>(response.Value);
            Assert.IsType<DistrictDto>(responseValue.Data);
            Assert.Equal(district.Id, responseValue.Data.Id);
        }


        [Fact]
        public async void Update_ActionExecute_ReturnSuccess()
        {
            var district = GetDistrictList().First();
            var carTypeDto = _mapper.Map<DistrictDto>(district);
            _mock.Setup(x => x.UpdateAsync(district));
            var result = await _districtsController.Update(carTypeDto);
            _mock.Verify(x => x.UpdateAsync(It.IsAny<District>()), Times.Exactly(1));
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);

        }



        [Fact]
        public async void Save_ActionExecute_ReturnSuccess()
        {
            var district = GetDistrictList().Find(x => x.Id == 1);
            _mock.Setup(m => m.AddAsync(It.IsAny<District>())).ReturnsAsync((District newDistrict) => newDistrict);
            var result = await _districtsController.Save(_mapper.Map<DistrictDto>(district));
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<DistrictDto>>(response.Value);
            Assert.IsType<DistrictDto>(responseValue.Data);
            Assert.Equal(district.Id, responseValue.Data.Id);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_ReturnSuccess(int id)
        {
            var district = GetDistrictList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(district);
            _mock.Setup(x => x.RemoveAsync(district));
            var result = await _districtsController.Remove(id);
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);
            _mock.Verify(x => x.RemoveAsync(district), Times.Once);
        }

        private List<District> GetDistrictList()
        {
            List<District> list = new List<District>()
            {
                new District {Id=1,Name="District-1" ,CityId=1},
                new District {Id=2,Name="District-2" ,CityId=2},
                new District {Id=3,Name="District-3" ,CityId=3},
            };
            return list;
        }
    }
}