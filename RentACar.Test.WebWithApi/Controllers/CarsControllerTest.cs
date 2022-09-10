using Microsoft.AspNetCore.Mvc;
using Moq;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Controllers;
using RentACar.WebWithApi.Service;

namespace RentACar.Test.WebWithApi.Controllers
{
    public class CarsControllerTest
    {
        private readonly Mock<IApiService> _mock;
        private readonly CarsController _carsController;

        public CarsControllerTest()
        {
            _mock = new Mock<IApiService>();
            _carsController = new CarsController(_mock.Object);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnView()
        {
            var result = await _carsController.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void Index_ActionExecutes_ReturnList()
        {
            _mock.Setup(x => x.GetAllAsync<CarWithFeatureDto>("Cars/GetCarWithFeature")).ReturnsAsync(GetCarWithFeatureList);
            var result = await _carsController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var list = Assert.IsAssignableFrom<List<CarWithFeatureDto>>(viewResult.Model);
            Assert.Equal<int>(3, list.Count());
        }

        [Fact]
        public async void SavePost_InValidModelState_NeverSaveMethodExecute()
        {

            _mock.Setup(x => x.GetAllAsync<CarTypeDto>("CarTypes")).ReturnsAsync(GetCarTypeList);
            _mock.Setup(x => x.GetAllAsync<RentalStoreDto>("RentalStores")).ReturnsAsync(GetRentalStoreList);
            _mock.Setup(x => x.GetAllAsync<ModelDto>("Models")).ReturnsAsync(GetModelList);
            
            _carsController.ModelState.AddModelError("", "");
            var result = await _carsController.Save(GetCarWithFeatureList().First());
            _mock.Verify(x => x.SaveAsync<CarDto>("Cars", It.IsAny<CarDto>()), Times.Never);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void SavePost_ValidModelState_SaveMethodExecute()
        {
            var carWithFeature = GetCarWithFeatureList().First();
            _mock.Setup(x => x.SaveAsync<CarDetailsDto>("CarDetails",carWithFeature.CarDetails)).ReturnsAsync(carWithFeature.CarDetails);
            _mock.Setup(x => x.SaveAsync<CarDto>("Cars",carWithFeature)).ReturnsAsync(carWithFeature);

            var result = await _carsController.Save(carWithFeature); 
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index",redirect.ActionName);
        }

        [Theory]
        [InlineData(3)]
        public async void Update_DataIsNotNull_ReturnView(int id)
        {
            var carWithFeature = GetCarWithFeatureList().Find(x=>x.Id==id);
            _mock.Setup(x => x.GetAllAsync<CarTypeDto>("CarTypes")).ReturnsAsync(GetCarTypeList);
            _mock.Setup(x => x.GetAllAsync<RentalStoreDto>("RentalStores")).ReturnsAsync(GetRentalStoreList);
            _mock.Setup(x => x.GetAllAsync<ModelDto>("Models")).ReturnsAsync(GetModelList);
            
            _mock.Setup(x => x.GetByIdAsync<CarWithFeatureDto>($"Cars/GetByIdCarWithFeature/{carWithFeature.Id}")).ReturnsAsync(carWithFeature);

            var result = await _carsController.Update(id);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CarWithFeatureDto>(viewResult.Model);
            Assert.Equal(carWithFeature.Id,model.Id);
        }

        [Fact]
        public async void UpdatePost_InValidModelState_ReturnViewWithData()
        {

            _mock.Setup(x => x.GetAllAsync<CarTypeDto>("CarTypes")).ReturnsAsync(GetCarTypeList);
            _mock.Setup(x => x.GetAllAsync<RentalStoreDto>("RentalStores")).ReturnsAsync(GetRentalStoreList);
            _mock.Setup(x => x.GetAllAsync<ModelDto>("Models")).ReturnsAsync(GetModelList);

            _carsController.ModelState.AddModelError("", "");
            var result = await _carsController.Update(GetCarWithFeatureList().First());
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CarWithFeatureDto>(viewResult.Model);
            Assert.Equal(GetCarWithFeatureList().First().Id,model.Id);
        }
        
        [Fact]
        public async void UpdatePost_ValidExecute_ReturnIndex()
        {
            var carWithFeature = GetCarWithFeatureList().First();
            var result = await _carsController.Update(carWithFeature);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }
        

        [Fact]
        public async void UpdatePost_ValidExecute_UpdateMethodExecute()
        {
            var carWithFeature = GetCarWithFeatureList().First();
            _mock.Setup(x => x.UpdateAsync<CarDto>("Cars", carWithFeature));
            _mock.Setup(x => x.UpdateAsync<CarDetailsDto>("CarDetails", carWithFeature.CarDetails));
            await _carsController.Update(carWithFeature);
            _mock.Verify(x => x.UpdateAsync<CarDto>("Cars", It.IsAny<CarDto>()), Times.Once);
            _mock.Verify(x => x.UpdateAsync<CarDetailsDto>("CarDetails", It.IsAny<CarDetailsDto>()), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async void Remove_ActionExecute_ReturnIndex(int id)
        {
            var carWithFeature = GetCarWithFeatureList().Find(x=>x.Id==id);
            _mock.Setup(x=>x.GetByIdAsync<CarDto>($"Cars/{carWithFeature.Id}")).ReturnsAsync(carWithFeature);
            var result = await _carsController.Remove(id);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Remove_ActionExecute_RemoveMethodExecute(int id)
        {

            var carWithFeature = GetCarWithFeatureList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetByIdAsync<CarDto>($"Cars/{carWithFeature.Id}")).ReturnsAsync(carWithFeature);

            _mock.Setup(x => x.RemoveAsync($"Cars/{carWithFeature.Id}"));
            _mock.Setup(x => x.RemoveAsync($"CarDetails/{carWithFeature.CarDetailsId}"));
            await _carsController.Remove(id);
            _mock.Verify(x => x.RemoveAsync($"Cars/{id}"), Times.Once); 
            _mock.Verify(x => x.RemoveAsync($"CarDetails/{carWithFeature.CarDetailsId}"), Times.Once); 
        }


        private List<CarWithFeatureDto> GetCarWithFeatureList() 
        {
            CarDetailsDto carDetailsDto = new CarDetailsDto {Id=1,Color="Test",CarYear=2000 };
            List<CarWithFeatureDto> list = new List<CarWithFeatureDto>()
            {
                new CarWithFeatureDto {Id=1,SeatCapacity=4,GearType="Manuel",FuelType="Gas",Price=500,CarTypeId=1,ModelId=1,CarDetailsId=1 ,CarDetails=carDetailsDto},
                new CarWithFeatureDto {Id=2,SeatCapacity=4,GearType="Manuel",FuelType="Gas",Price=500,CarTypeId=1,ModelId=1,CarDetailsId=1 ,CarDetails=carDetailsDto},
                new CarWithFeatureDto {Id=3,SeatCapacity=4,GearType="Manuel",FuelType="Gas",Price=500,CarTypeId=1,ModelId=1,CarDetailsId=1 ,CarDetails=carDetailsDto},
                
            };
            return list;
        }

        private List<CarTypeDto> GetCarTypeList()
        {
            List<CarTypeDto> list = new List<CarTypeDto>()
            {
                new CarTypeDto {Id=1,Name="Sedan" },
                new CarTypeDto {Id=2,Name="Hatchback" },
                new CarTypeDto {Id=3,Name="Cuv" },
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

        private List<ModelDto> GetModelList()
        {
            List<ModelDto> list = new List<ModelDto>()
            {
                new ModelDto {Id=1,Name="Test-1" },
                new ModelDto {Id=2,Name="Test-2" },
                new ModelDto {Id=3,Name="Test-3" }
            };
            return list;
        }
    }
}
