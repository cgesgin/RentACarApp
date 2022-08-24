using Microsoft.AspNetCore.Mvc;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Service;

namespace RentACar.WebWithApi.Controllers
{
    public class CarTypesController : Controller
    {
        private readonly ApiService _apiService;

        public CarTypesController(ApiService apiService)
        {
            _apiService = apiService;
        }
        public async Task<IActionResult> Index()
        {
            var carTypes = await _apiService.GetAllAsync<CarTypeDto>("CarTypes");
            return View(carTypes);
        }

        public IActionResult Save()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(CarTypeDto carTypeDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.SaveAsync<CarTypeDto>("CarTypes", carTypeDto);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Update(int id)
        {
            var carType = await _apiService.GetByIdAsync<CarTypeDto>($"CarTypes/{id}");
            return View(carType);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CarTypeDto carTypeDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateAsync<CarTypeDto>("CarTypes", carTypeDto);
                return RedirectToAction(nameof(Index));
            }
            return View(carTypeDto);
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _apiService.RemoveAsync($"CarTypes/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
