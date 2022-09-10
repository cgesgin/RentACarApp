using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Service;

namespace RentACar.WebWithApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DistrictsController : Controller
    {
        private readonly IApiService _apiService;

        public DistrictsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var districts = await _apiService.GetAllAsync<DistrictDto>("Districts");
            return View(districts);
        }
        
        public async Task<IActionResult> Save()
        {
            var cities = await _apiService.GetAllAsync<CityDto>("Cities");
            ViewBag.cities = new SelectList(cities, "Id", "Name");
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Save(DistrictDto districtDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.SaveAsync<DistrictDto>("Districts", districtDto);
                return RedirectToAction(nameof(Index));
            }
            var cities = await _apiService.GetAllAsync<CityDto>("Cities");
            ViewBag.cities = new SelectList(cities, "Id", "Name");
            return View();
        }
        
        public async Task<IActionResult> Update(int id)
        {
            var district = await _apiService.GetByIdAsync<DistrictDto>($"Districts/{id}");
            var cities = await _apiService.GetAllAsync<CityDto>("Cities");
            ViewBag.cities = new SelectList(cities, "Id", "Name");
            return View(district);
        }

        [HttpPost]
        public async Task<IActionResult> Update(DistrictDto districtDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateAsync<DistrictDto>("Districts", districtDto);
                return RedirectToAction(nameof(Index));
            }
            var cities = await _apiService.GetAllAsync<CityDto>("Cities");
            ViewBag.cities = new SelectList(cities, "Id", "Name", districtDto.CityId);
            return View(districtDto);
        }

        public async Task<IActionResult> Remove(int id)
        {
            var address = await _apiService.RemoveAsync($"Districts/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
