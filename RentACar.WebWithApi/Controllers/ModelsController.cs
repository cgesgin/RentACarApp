using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Service;

namespace RentACar.WebWithApi.Controllers
{
    public class ModelsController : Controller
    {
        private readonly ApiService _apiService;

        public ModelsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var costumers = await _apiService.GetAllAsync<ModelWithBrandDto>("Models/GetModelWithBrand");
            return View(costumers);
        }

        public async Task<IActionResult> Save()
        {
            var brands = await _apiService.GetAllAsync<BrandDto>("Brands");
            ViewBag.brands = new SelectList(brands, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ModelDto modelDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.SaveAsync<ModelDto>("Models", modelDto);
                return RedirectToAction(nameof(Index));
            }
            var brands = await _apiService.GetAllAsync<BrandDto>("Brands");
            ViewBag.brands = new SelectList(brands, "Id", "Name");
            return View();
        }

        public async Task<IActionResult> Update(int id)
        {
            var carModel = await _apiService.GetByIdAsync<ModelDto>($"Models/{id}");
            var brands = await _apiService.GetAllAsync<BrandDto>("Brands");
            ViewBag.brands = new SelectList(brands, "Id", "Name", carModel.BrandId);
            return View(carModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ModelDto modelDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateAsync<ModelDto>("Models",modelDto);
                return RedirectToAction(nameof(Index));
            }
            var brands = await _apiService.GetAllAsync<BrandDto>("Brands");
            ViewBag.brands = new SelectList(brands, "Id", "Name", modelDto.BrandId);
            return View(modelDto);
        }

        public async Task<IActionResult> Remove(int id)
        {
            var carModel = await _apiService.RemoveAsync($"Models/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
