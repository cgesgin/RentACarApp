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
    public class ModelsControllerTest
    {
        private readonly Mock<IModelService> _mock;
        private readonly ModelsController _modelsController;
        private readonly IMapper _mapper;

        public ModelsControllerTest()
        {
            _mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new MapProfile())));
            _mock = new Mock<IModelService>();
            _modelsController = new ModelsController(_mapper, _mock.Object);
        }

        [Fact]
        public async void GetAll_ActionExecute_ReturnSuccess()
        {
            _mock.Setup(x => x.GetAllAsync()).ReturnsAsync(GetModelList());
            var result = await _modelsController.GetAll();
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, response.StatusCode);
            var responseValue = Assert.IsType<ResponseDto<List<ModelDto>>>(response.Value);
            Assert.Equal(3, responseValue.Data.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetById_ActionExecute_ReturnSuccess(int id)
        {
            var model = GetModelList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(model);
            var result = await _modelsController.GetById(id);
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<ModelDto>>(response.Value);
            Assert.IsType<ModelDto>(responseValue.Data);
            Assert.Equal(model.Id, responseValue.Data.Id);
        }


        [Fact]
        public async void Update_ActionExecute_ReturnSuccess()
        {
            var model = GetModelList().First();
            var modelDto = _mapper.Map<ModelDto>(model);
            _mock.Setup(x => x.UpdateAsync(model));
            var result = await _modelsController.Update(modelDto);
            _mock.Verify(x => x.UpdateAsync(It.IsAny<Model>()), Times.Exactly(1));
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);

        }



        [Fact]
        public async void Save_ActionExecute_ReturnSuccess()
        {
            var model = GetModelList().Find(x => x.Id == 1);
            _mock.Setup(m => m.AddAsync(It.IsAny<Model>())).ReturnsAsync((Model newModel) => newModel);
            var result = await _modelsController.Save(_mapper.Map<ModelDto>(model));
            var response = Assert.IsType<ObjectResult>(result);
            var responseValue = Assert.IsType<ResponseDto<ModelDto>>(response.Value);
            Assert.IsType<ModelDto>(responseValue.Data);
            Assert.Equal(model.Id, responseValue.Data.Id);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_ReturnSuccess(int id)
        {
            var model = GetModelList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(model);
            _mock.Setup(x => x.RemoveAsync(model));
            var result = await _modelsController.Remove(id);
            var response = Assert.IsType<ObjectResult>(result);
            Assert.Equal(204, response.StatusCode);
            Assert.Equal(null, response.Value);
            _mock.Verify(x => x.RemoveAsync(model), Times.Once);
        }

        private List<Model> GetModelList()
        {
            List<Model> list = new List<Model>()
            {
                new Model {Id=1,Name="Model-1" ,BrandId=1},
                new Model {Id=2,Name="Model-2" ,BrandId=2},
                new Model {Id=3,Name="Model-3" ,BrandId=3},
            };
            return list;
        }
    }
}