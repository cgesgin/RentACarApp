using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Service;

namespace RentACar.WebWithApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AddressesController : Controller
    {
        private readonly ApiService _apiService;

        public AddressesController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var addresses = await _apiService.GetAllAsync<AddressWithDistrictDto>("Addresses/GetAddressWithDistrict");
            return View(addresses);
        }
        public async Task<IActionResult> Save()
        {
            var districts = await _apiService.GetAllAsync<DistrictDto>("Districts");
            ViewBag.districts = new SelectList(districts, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(AddressDto addressDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.SaveAsync<AddressDto>("Addresses", addressDto);
                return RedirectToAction(nameof(Index));
            }
            var districts = await _apiService.GetAllAsync<DistrictDto>("Districts");
            ViewBag.districts = new SelectList(districts, "Id", "Name");
            return View();
        }

        public async Task<IActionResult> Update(int id)
        {
            var address = await _apiService.GetByIdAsync<AddressDto>($"Addresses/{id}");
            var districts = await _apiService.GetAllAsync<DistrictDto>("Districts");
            ViewBag.districts = new SelectList(districts, "Id", "Name");
            return View(address);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AddressDto addressDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateAsync<AddressDto>("Addresses", addressDto);
                return RedirectToAction(nameof(Index));
            }
            var districts = await _apiService.GetAllAsync<DistrictDto>("Districts");
            ViewBag.districts = new SelectList(districts, "Id", "Name",addressDto.DistrictId);
            return View(addressDto);
        }

        public async Task<IActionResult> Remove(int id)
        {
            var address = await _apiService.RemoveAsync($"Addresses/{id}");
            return RedirectToAction(nameof(Index));
        }

    }
}
