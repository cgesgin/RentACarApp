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
        private readonly IApiService _apiService;
        private readonly UserManager<IdentityUser> _userManager;

        public RentalsController(IApiService apiService, UserManager<IdentityUser> userManager)
        {
            _apiService = apiService;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var rentals = await _apiService.GetAllAsync<RentalWithCarAndCostumerDto>($"Rentals/GetByUserIdWithCarAndCostumer/{user.Id}");
            return View(rentals);
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AdminIndex()
        {
            var rentals = await _apiService.GetAllAsync<RentalWithCarAndCostumerDto>($"Rentals/GetRentalWithCarAndCostumer");
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
            var car = await _apiService.GetByIdAsync<CarWithFeatureDto>($"Cars/GetByIdCarWithFeature/{rentalWithCostumerDto.CarId}");
            if (ModelState.IsValid)
            {
                var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
                rentalWithCostumerDto.Costumer.UserId = user.Id;
                var costumer = await _apiService.SaveAsync<CostumerDto>("Costumers", rentalWithCostumerDto.Costumer);
                rentalWithCostumerDto.CostumerId = costumer.Id;
                var rental = await _apiService.SaveAsync<RentalDto>("Rentals", rentalWithCostumerDto);
                car.Status = "rented";
                await _apiService.UpdateAsync<CarDto>("Cars", car);
                return Redirect($"~/Payments/Save/{rental.Id}");
            }
            ViewBag.car = car;
            var rentalStores = await _apiService.GetAllAsync<RentalStoreDto>("RentalStores");
            ViewBag.rentalStores = new SelectList(rentalStores, "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int? id)
        {
            var rental = await _apiService.GetByIdAsync<RentalDto>($"Rentals/{id}");
            var rentalStores = await _apiService.GetAllAsync<RentalStoreDto>("RentalStores");
            ViewBag.rentalStores = new SelectList(rentalStores, "Id", "Name");
            return View(rental);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(RentalDto rentalDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateAsync<RentalDto>("Rentals",rentalDto);
                return Redirect(nameof(AdminIndex));
            }
            return View();
        }

    }
}
