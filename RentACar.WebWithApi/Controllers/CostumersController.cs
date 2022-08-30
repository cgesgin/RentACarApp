using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Service;

namespace RentACar.WebWithApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CostumersController : Controller
    {
        private readonly ApiService _apiService;

        public CostumersController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var costumers = await _apiService.GetAllAsync<CostumerDto>("Costumers");
            return View(costumers);
        }

        public IActionResult Save()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(CostumerDto costumerDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.SaveAsync<CostumerDto>("Costumers", costumerDto);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Update(int id)
        {
            var costumer = await _apiService.GetByIdAsync<CostumerDto>($"Costumers/{id}");
            return View(costumer);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CostumerDto costumerDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateAsync<CostumerDto>("Costumers", costumerDto);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _apiService.RemoveAsync($"Costumers/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}