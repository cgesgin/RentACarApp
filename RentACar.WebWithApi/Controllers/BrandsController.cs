using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Service;

namespace RentACar.WebWithApi.Controllers
{
    [Authorize(Roles ="Admin")]
    public class BrandsController : Controller
    {
        private readonly IApiService _apiService;

        public BrandsController(IApiService apiService)
        {
            _apiService = apiService;
        }
        public async Task<IActionResult> Index()
        {
            var brands = await _apiService.GetAllAsync<BrandDto>("Brands");
            return View(brands);
        }
        public IActionResult Save()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Save(BrandDto brandDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.SaveAsync<BrandDto>("Brands", brandDto);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Update(int id)
        {
            var address = await _apiService.GetByIdAsync<BrandDto>($"Brands/{id}");
            return View(address);
        }

        [HttpPost]
        public async Task<IActionResult> Update(BrandDto brandDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateAsync<BrandDto>("Brands", brandDto);
                return RedirectToAction(nameof(Index));
            }
            return View(brandDto);
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _apiService.RemoveAsync($"Brands/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
