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
    public class BrandsControllerTest
    {
        private readonly Mock<IBrandService> _mock;
        private readonly BrandsController _brandsController;
        private readonly IMapper _mapper;

        public BrandsControllerTest()
        {
            _mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new MapProfile())));
            _mock = new Mock<IBrandService>();
            _brandsController = new BrandsController(_mapper, _mock.Object);
        }


        [Fact]
        public async void GetAll_ActionExecute_ReturnSuccess()
        {
            _mock.Setup(x => x.GetAllAsync()).ReturnsAsync(GetBrandList());
            var result = await _brandsController.GetAll();
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, response.StatusCode);
            var responseValue = Assert.IsType<ResponseDto<List<BrandDto>>>(response.Value);
            Assert.Equal(3, responseValue.Data.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetById_ActionExecute_ReturnSuccess(int id)
        {
            var brand = GetBrandList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(brand);
            var result = await _brandsController.GetById(id);
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<BrandDto>>(response.Value);
            Assert.IsType<BrandDto>(responseValue.Data);
            Assert.Equal(brand.Id, responseValue.Data.Id);
        }

        [Fact]
        public async void Update_ActionExecute_ReturnSuccess()
        {
            var brand = GetBrandList().First();
            var brandDto = _mapper.Map<BrandDto>(brand);
            _mock.Setup(x => x.UpdateAsync(brand));
            var result = await _brandsController.Update(brandDto);
            _mock.Verify(x => x.UpdateAsync(It.IsAny<Brand>()), Times.Exactly(1));
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);

        }

        [Fact]
        public async void Save_ActionExecute_ReturnSuccess()
        {
            var brand = GetBrandList().Find(x => x.Id == 1);
            _mock.Setup(m => m.AddAsync(It.IsAny<Brand>())).ReturnsAsync((Brand newBrand) => newBrand);
            var result = await _brandsController.Save(_mapper.Map<BrandDto>(brand));
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<BrandDto>>(response.Value);
            Assert.IsType<BrandDto>(responseValue.Data);
            Assert.Equal(brand.Id, responseValue.Data.Id);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_ReturnSuccess(int id)
        {
            var brand = GetBrandList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(brand);
            _mock.Setup(x => x.RemoveAsync(brand));
            var result = await _brandsController.Remove(id);
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);
            _mock.Verify(x => x.RemoveAsync(brand), Times.Once);
        }



        private List<Brand> GetBrandList()
        {
            List<Brand> list = new List<Brand>()
            {
                new Brand {Id=1,Name="BMW" },
                new Brand {Id=2,Name="Audi" },
                new Brand {Id=3,Name="Aston Martini" },
            };
            return list;
        }
    }
}
