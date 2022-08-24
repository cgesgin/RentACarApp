using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Service;

namespace RentACar.WebWithApi.Controllers
{
    public class RentalStoresController : Controller
    {
        private readonly ApiService _apiService;

        public RentalStoresController(ApiService apiService)
        {
            _apiService = apiService;
        }
        public async Task<IActionResult> Index()
        {
            var rentalStores = await _apiService.GetAllAsync<RentalStoreWithAddressDto>("RentalStores/GetRentalStoreWithAddress");
            return View(rentalStores);
        }
        public async Task<IActionResult> Save()
        {
            var brands = await _apiService.GetAllAsync<DistrictDto>("Districts");
            ViewBag.districts = new SelectList(brands, "Id", "Name");
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Save(RentalStoreWithAddressDto rentalStore)
        {
            if (ModelState.IsValid)
            {
                var address = await _apiService.SaveAsync<AddressDto>("Addresses", rentalStore.Address);
                rentalStore.AddressId = address.Id;
                await _apiService.SaveAsync<RentalStoreDto>("RentalStores", rentalStore);
                return RedirectToAction(nameof(Index));
            }
            var brands = await _apiService.GetAllAsync<DistrictDto>("Districts");
            ViewBag.districts = new SelectList(brands, "Id", "Name");
            return View();
        }

        public async Task<IActionResult> Update(int id)
        {
            var rentalStore = await _apiService.GetByIdAsync<RentalStoreWithAddressDto>($"RentalStores/GetByIdRentalStoreWithAddress/{id}");
            var districts = await _apiService.GetAllAsync<DistrictDto>("Districts");
            ViewBag.districts = new SelectList(districts, "Id", "Name");
            return View(rentalStore);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RentalStoreWithAddressDto rentalStoreWithAddressDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateAsync<RentalStoreDto>("RentalStores", rentalStoreWithAddressDto);
                await _apiService.UpdateAsync<AddressDto>("Addresses", rentalStoreWithAddressDto.Address);
                return RedirectToAction(nameof(Index));
            }
            var districts = await _apiService.GetAllAsync<DistrictDto>("Districts");
            ViewBag.districts = new SelectList(districts, "Id", "Name", rentalStoreWithAddressDto.AddressId);
            return View(rentalStoreWithAddressDto);
        }

        public async Task<IActionResult> Remove(int id)
        {
            var rentalStore = await _apiService.GetByIdAsync<RentalStoreDto>($"RentalStores/{id}");
            await _apiService.RemoveAsync($"RentalStores/{id}");
            await _apiService.RemoveAsync($"Addresses/{rentalStore.AddressId}");
            return RedirectToAction(nameof(Index));
        }
    }
}
