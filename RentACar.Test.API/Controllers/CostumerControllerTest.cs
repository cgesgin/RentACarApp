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
    public class CostumerControllerTest
    {
        private readonly Mock<ICostumerService> _mock;
        private readonly CostumersController _costumerController;
        private readonly IMapper _mapper;

        public CostumerControllerTest()
        {
            _mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new MapProfile())));
            _mock = new Mock<ICostumerService>();
            _costumerController = new CostumersController(_mapper, _mock.Object);
        }

        [Fact]
        public async void GetAll_ActionExecute_ReturnSuccess()
        {
            var costumers = _mock.Setup(x => x.GetAllAsync()).ReturnsAsync(GetCostumerList());
            var result = await _costumerController.GetAll();
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, response.StatusCode);
            var responseValue = Assert.IsType<ResponseDto<List<CostumerDto>>>(response.Value);
            Assert.Equal(3, responseValue.Data.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetById_ActionExecute_ReturnSuccess(int id)
        {
            var costumer = GetCostumerList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(costumer);
            var result = await _costumerController.GetById(id);
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<CostumerDto>>(response.Value);
            Assert.IsType<CostumerDto>(responseValue.Data);
            Assert.Equal(costumer.Id, responseValue.Data.Id);
        }

        [Fact]
        public async void Update_ActionExecute_ReturnSuccess()
        {
            var costumer = GetCostumerList().First();

            var costumerDto = _mapper.Map<CostumerDto>(costumer);
            _mock.Setup(x => x.UpdateAsync(costumer));
            var result = await _costumerController.Update(costumerDto);

            _mock.Verify(x => x.UpdateAsync(It.IsAny<Costumer>()), Times.Once);
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);

        }

        [Fact]
        public async void Save_ActionExecute_ReturnSuccess()
        {
            var costumer = GetCostumerList().Find(x=>x.Id==1);            
            _mock.Setup(m => m.AddAsync(It.IsAny<Costumer>())).ReturnsAsync((Costumer newCostumer) => newCostumer);
            var result = await _costumerController.Save(_mapper.Map<CostumerDto>(costumer));            
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<CostumerDto>>(response.Value);
            Assert.IsType<CostumerDto>(responseValue.Data);
            Assert.Equal(costumer.Id, responseValue.Data.Id);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_ReturnSuccess(int id)
        {

            var costumer = GetCostumerList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(costumer);
            _mock.Setup(x => x.RemoveAsync(costumer));
            var result = await _costumerController.Remove(id);
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);
            _mock.Verify(x => x.RemoveAsync(costumer), Times.Once);
        }


        private List<Costumer> GetCostumerList()
        {
            List<Costumer> list = new List<Costumer>()
            {
                new Costumer {Id=1,Name="John",LastName="Doe",Email="john@doe.com",Phone="555-555-5555" ,LisanceNo="02102100" },
                new Costumer {Id=2,Name="Jane",LastName="Doe",Email="jane@doe.com",Phone="555-555-4444" ,LisanceNo="02102101" },
                new Costumer {Id=3,Name="Hall",LastName="Doe",Email="hale@doe.com",Phone="555-555-3333" ,LisanceNo="02102102" }
            };
            return list;
        }
    }
}
