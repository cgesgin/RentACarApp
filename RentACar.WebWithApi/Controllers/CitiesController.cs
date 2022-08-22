using Microsoft.AspNetCore.Mvc;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Service;

namespace RentACar.WebWithApi.Controllers
{
    public class CitiesController : Controller
    {
        private readonly ApiService _apiService;

        public CitiesController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var cities = await _apiService.GetAllAsync<CityDto>("Cities");
            return View(cities);
        }

        public IActionResult Save()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Save(CityDto cityDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.SaveAsync<CityDto>("Cities", cityDto);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> Update(int id)
        {
            var city = await _apiService.GetByIdAsync<CityDto>($"Cities/{id}");
            return View(city);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CityDto cityDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateAsync<CityDto>("Cities", cityDto);
                return RedirectToAction(nameof(Index));
            }
            return View(cityDto);
        }

        public async Task<IActionResult> Remove(int id)
        {
            var address = await _apiService.RemoveAsync($"Cities/{id}");
            return RedirectToAction(nameof(Index));
        }

    }
}
