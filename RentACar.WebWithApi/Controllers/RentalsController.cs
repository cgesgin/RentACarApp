using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Service;

namespace RentACar.WebWithApi.Controllers
{
    [Authorize]
    public class RentalsController : Controller
    {
        private readonly ApiService _apiService;
        private readonly UserManager<IdentityUser> _userManager;

        public RentalsController(ApiService apiService, UserManager<IdentityUser> userManager)
        {
            _apiService = apiService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var rentals= await _apiService.GetAllAsync<RentalWithCarAndCostumerDto>($"Rentals/GetByUserIdWithCarAndCostumer/{user.Id}");
            return View(rentals);
        }

        public async Task<IActionResult> Save(int ? id) 
        {
            ViewBag.car = await _apiService.GetByIdAsync<CarWithFeatureDto>($"Cars/GetByIdCarWithFeature/{id}");
            var rentalStores = await _apiService.GetAllAsync<RentalStoreDto>("RentalStores");
            ViewBag.rentalStores = new SelectList(rentalStores, "Id", "Name");
            return View();
        }
 
        [HttpPost]
        public async Task<IActionResult> Save(RentalWithCostumerDto rentalWithCostumerDto)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
                rentalWithCostumerDto.Costumer.UserId = user.Id;
                var costumer = await _apiService.SaveAsync<CostumerDto>("Costumers", rentalWithCostumerDto.Costumer);
                rentalWithCostumerDto.CostumerId = costumer.Id;
                var rental = await _apiService.SaveAsync<RentalDto>("Rentals", rentalWithCostumerDto);
                return Redirect($"~/Payments/Save/{rental.Id}");
            }
            ViewBag.car = await _apiService.GetByIdAsync<CarWithFeatureDto>($"Cars/GetByIdCarWithFeature/{rentalWithCostumerDto.CarId}");
            var rentalStores = await _apiService.GetAllAsync<RentalStoreDto>("RentalStores");
            ViewBag.rentalStores = new SelectList(rentalStores, "Id", "Name");
            return View();
        }
    }
}
