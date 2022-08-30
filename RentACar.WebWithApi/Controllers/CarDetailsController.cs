using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Service;

namespace RentACar.WebWithApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CarDetailsController : Controller
    {
        private readonly ApiService _apiService;

        public CarDetailsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var carDetails = await _apiService.GetAllAsync<CarDetailsDto>("CarDetails");
            return View(carDetails);
        }

        public IActionResult Save()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(CarDetailsDto carDetailsDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.SaveAsync<CarDetailsDto>("CarDetails", carDetailsDto);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Update(int id)
        {
            var carDetails = await _apiService.GetByIdAsync<CarDetailsDto>($"CarDetails/{id}");
            return View(carDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CarDetailsDto carDetailsDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateAsync<CarDetailsDto>("CarDetails", carDetailsDto);
                return RedirectToAction(nameof(Index));
            }
            return View(carDetailsDto);
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _apiService.RemoveAsync($"CarDetails/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
