using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Controllers;
using RentACar.WebWithApi.Service;
using System.Security.Claims;

namespace RentACar.Test.WebWithApi.Controllers
{
    public class RentalsControllerTest
    {
        private readonly Mock<IApiService> _mock;
        private readonly RentalsController _rentalsController;
        private readonly Mock<IUserStore<IdentityUser>> _mockStore;
        private readonly UserManager<IdentityUser> _userManager;

        public RentalsControllerTest()
        {
            _mock = new Mock<IApiService>();
            _mockStore = new Mock<IUserStore<IdentityUser>>();
            _userManager = new UserManager<IdentityUser>(_mockStore.Object, null, null, null, null, null, null, null, null);

            _rentalsController = new RentalsController(_mock.Object, _userManager);
        }

        [Fact]
        public async Task Index_IsUserNotNull_ReturnView()
        {
            _mockStore.Setup(x => x.FindByNameAsync(Authentication().Identity.Name, CancellationToken.None))
            .ReturnsAsync(new IdentityUser()
            {
                UserName = "test@email.com",
                Id = "123"
            });
            var result = await _rentalsController.Index();
            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public async void Index_ActionExecutes_ReturnList()
        {
            _mockStore.Setup(x => x.FindByNameAsync(Authentication().Identity.Name, CancellationToken.None))
            .ReturnsAsync(new IdentityUser()
            {
                UserName = "test@email.com",
                Id = "123"
            });
            string userId = "123";
            _mock.Setup(x => x.GetAllAsync<RentalWithCarAndCostumerDto>($"Rentals/GetByUserIdWithCarAndCostumer/{userId}")).ReturnsAsync(GetRentalWithCarAndCostumerList);
            var result = await _rentalsController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var list = Assert.IsAssignableFrom<List<RentalWithCarAndCostumerDto>>(viewResult.Model);
            Assert.Equal<int>(3, list.Count());
        }

        [Fact]
        public async void AdminIndex_ActionExecutes_ReturnView()
        {
            _mock.Setup(x => x.GetAllAsync<RentalWithCarAndCostumerDto>($"Rentals/GetRentalWithCarAndCostumer")).ReturnsAsync(GetRentalWithCarAndCostumerList);
            var result = await _rentalsController.AdminIndex();
            var viewResult = Assert.IsType<ViewResult>(result);
            var list = Assert.IsAssignableFrom<List<RentalWithCarAndCostumerDto>>(viewResult.Model);
            Assert.Equal<int>(3, list.Count());
        }

        [Theory]
        [InlineData(1)]
        public async void Save_ActionExecutes_ReturnView(int id)
        {
            var car = GetCarWithFeatureDtoList().Find(x => x.Id == id);
            _mock.Setup(x => x.GetAllAsync<RentalStoreDto>("RentalStores")).ReturnsAsync(GetRentalStoreList);
            _mock.Setup(x => x.GetByIdAsync<CarWithFeatureDto>($"Cars/GetByIdCarWithFeature/{car.Id}")).ReturnsAsync(car);
            var result = await _rentalsController.Save(id);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void SavePost_ActionExecutes_ReturnRedirect()
        {
            var rentalWithCostumer = GetRentalWithCostumerDtoList().First();
            var car = GetCarWithFeatureDtoList().Find(x=>x.Id== rentalWithCostumer.CarId);

            _mock.Setup(x => x.GetByIdAsync<CarWithFeatureDto>($"Cars/GetByIdCarWithFeature/{car.Id}")).ReturnsAsync(car);
            _mockStore.Setup(x => x.FindByNameAsync(Authentication().Identity.Name, CancellationToken.None))
            .ReturnsAsync(new IdentityUser()
            {
                UserName = "test@email.com",
                Id = "123"
            });
            _mock.Setup(x => x.SaveAsync<CostumerDto>("Costumers", rentalWithCostumer.Costumer)).ReturnsAsync(rentalWithCostumer.Costumer);
            _mock.Setup(x =>x.SaveAsync<RentalDto>("Rentals", rentalWithCostumer)).ReturnsAsync(rentalWithCostumer);
            _mock.Setup(x =>x.UpdateAsync<CarDto>("Cars", car));
            var result = await _rentalsController.Save(rentalWithCostumer);
            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public async void SavePost_InValidActionExecutes_ReturnView()
        {
            var rentalWithCostumer = GetRentalWithCostumerDtoList().First();
            var car = GetCarWithFeatureDtoList().Find(x => x.Id == rentalWithCostumer.CarId);

            _mock.Setup(x => x.GetByIdAsync<CarWithFeatureDto>($"Cars/GetByIdCarWithFeature/{car.Id}")).ReturnsAsync(car);
            _mock.Setup(x => x.GetAllAsync<RentalStoreDto>("RentalStores")).ReturnsAsync(GetRentalStoreList);
            _rentalsController.ModelState.AddModelError("", "");
            var result = await _rentalsController.Save(rentalWithCostumer);
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void Update_DataIsNotNull_ReturnView(int id) 
        {
            var rental= GetRentalWithCostumerDtoList().Find(x=>x.Id==id);
            _mock.Setup(x=>x.GetByIdAsync<RentalDto>($"Rentals/{rental.Id}")).ReturnsAsync(rental);
            _mock.Setup(x => x.GetAllAsync<RentalStoreDto>("RentalStores")).ReturnsAsync(GetRentalStoreList);
            var result = await _rentalsController.Update(id);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RentalDto>(viewResult.Model);
            Assert.Equal<int>(rental.Id, model.Id);
        }

        [Fact]
        public async void UpdatePost_InValid_ReturnView()
        {
            var rental = GetRentalWithCostumerDtoList().First();
            _rentalsController.ModelState.AddModelError("","");
            var result = await _rentalsController.Update(rental);
            Assert.IsType<ViewResult>(result);
        }

        private ClaimsPrincipal Authentication()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,"test4@email.com"),
                    new Claim(ClaimTypes.NameIdentifier, "1234")
                }, "mock"));

            _rentalsController.ControllerContext.HttpContext = new DefaultHttpContext() { User = user };
            return user;
        }

        private List<RentalWithCostumerDto> GetRentalWithCostumerDtoList()
        {
            CostumerDto costumerDto = new CostumerDto { Id = 1, Name = "John", LastName = "Doe", Email = "john@doe.com", Phone = "555-555-5555", LisanceNo = "02102100" };
            List<RentalWithCostumerDto> list = new List<RentalWithCostumerDto>()
            {
                new RentalWithCostumerDto { Id =1,RentalDate=DateTime.Now,ReturnDate=DateTime.Now,TotalAmount=100,RentalAmount=120 ,Costumer = costumerDto ,CarId=1},
                new RentalWithCostumerDto { Id =2,RentalDate=DateTime.Now,ReturnDate=DateTime.Now,TotalAmount=100,RentalAmount=120 ,Costumer = costumerDto ,CarId=2},
                new RentalWithCostumerDto { Id =3,RentalDate=DateTime.Now,ReturnDate=DateTime.Now,TotalAmount=100,RentalAmount=120 ,Costumer = costumerDto ,CarId=3},
            };
            return list;
        }

        private List<RentalWithCarAndCostumerDto> GetRentalWithCarAndCostumerList()
        {
            List<RentalWithCarAndCostumerDto> list = new List<RentalWithCarAndCostumerDto>()
            {
                new RentalWithCarAndCostumerDto { Id =1,RentalDate=DateTime.Now,ReturnDate=DateTime.Now,TotalAmount=100,RentalAmount=120 ,CarId=1 },
                new RentalWithCarAndCostumerDto { Id =2,RentalDate=DateTime.Now,ReturnDate=DateTime.Now,TotalAmount=100,RentalAmount=120 ,CarId=2},
                new RentalWithCarAndCostumerDto { Id =3,RentalDate=DateTime.Now,ReturnDate=DateTime.Now,TotalAmount=100,RentalAmount=120 ,CarId=3},
            };
            return list;
        }

        private List<CarWithFeatureDto> GetCarWithFeatureDtoList()
        {
            List<CarWithFeatureDto> list = new List<CarWithFeatureDto>()
            {
                new CarWithFeatureDto {Id=1,SeatCapacity=4,GearType="Manuel",FuelType="Gas",Price=500,CarTypeId=1,ModelId=1,CarDetailsId=1  },
                new CarWithFeatureDto {Id=2,SeatCapacity=4,GearType="Manuel",FuelType="Gas",Price=500,CarTypeId=1,ModelId=1,CarDetailsId=1  },
                new CarWithFeatureDto {Id=3,SeatCapacity=4,GearType="Manuel",FuelType="Gas",Price=500,CarTypeId=1,ModelId=1,CarDetailsId=1  },
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
    }
}
