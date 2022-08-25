using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Service;

namespace RentACar.WebWithApi.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApiService _apiService;

        public CarsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await _apiService.GetAllAsync<CarWithFeatureDto>("Cars/GetCarWithFeature");
            return View(cars);
        }

        public async Task<IActionResult> Save()
        {
            var carTypes = await _apiService.GetAllAsync<CarTypeDto>("CarTypes");
            ViewBag.carTypes = new SelectList(carTypes, "Id", "Name");

            var rentalStores = await _apiService.GetAllAsync<RentalStoreDto>("RentalStores");
            ViewBag.rentalStores = new SelectList(rentalStores, "Id", "Name");

            var models = await _apiService.GetAllAsync<ModelDto>("Models");
            ViewBag.models = new SelectList(models, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(CarWithFeatureDto carWithFeatureDto)
        {
            if (ModelState.IsValid)
            {
                var carDetails = await _apiService.SaveAsync<CarDetailsDto>("CarDetails", carWithFeatureDto.CarDetails);
                carWithFeatureDto.CarDetailsId = carDetails.Id;
                await _apiService.SaveAsync<CarDto>("Cars", carWithFeatureDto);                
                return RedirectToAction(nameof(Index));
            }

            var carTypes = await _apiService.GetAllAsync<CarTypeDto>("CarTypes");
            ViewBag.carTypes = new SelectList(carTypes, "Id", "Name");

            var rentalStores = await _apiService.GetAllAsync<RentalStoreDto>("RentalStores");
            ViewBag.rentalStores = new SelectList(rentalStores, "Id", "Name");

            var models = await _apiService.GetAllAsync<ModelDto>("Models");
            ViewBag.models = new SelectList(models, "Id", "Name");
            return View();
        }

        public async Task<IActionResult> Update(int id)
        {
            var cars = await _apiService.GetByIdAsync<CarWithFeatureDto>($"Cars/GetByIdCarWithFeature/{id}");
            
            var carTypes = await _apiService.GetAllAsync<CarTypeDto>("CarTypes");
            ViewBag.carTypes = new SelectList(carTypes, "Id", "Name");

            var rentalStores = await _apiService.GetAllAsync<RentalStoreDto>("RentalStores");
            ViewBag.rentalStores = new SelectList(rentalStores, "Id", "Name");

            var models = await _apiService.GetAllAsync<ModelDto>("Models");
            ViewBag.models = new SelectList(models, "Id", "Name");

            return View(cars);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CarWithFeatureDto carWithFeatureDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateAsync<CarDto>("Cars", carWithFeatureDto);
                await _apiService.UpdateAsync<CarDetailsDto>("CarDetails", carWithFeatureDto.CarDetails);
                return RedirectToAction(nameof(Index));
            }
            var carTypes = await _apiService.GetAllAsync<CarTypeDto>("CarTypes");
            ViewBag.carTypes = new SelectList(carTypes, "Id", "Name",carWithFeatureDto.CarTypeId);

            var rentalStores = await _apiService.GetAllAsync<RentalStoreDto>("RentalStores");
            ViewBag.rentalStores = new SelectList(rentalStores, "Id", "Name", carWithFeatureDto.RentalStoreId);

            var models = await _apiService.GetAllAsync<ModelDto>("Models");
            ViewBag.models = new SelectList(models, "Id", "Name",carWithFeatureDto.ModelId);
            return View(carWithFeatureDto);
        }

        public async Task<IActionResult> Remove(int id)
        {
            var cars = await _apiService.GetByIdAsync<CarDto>($"Cars/{id}");
            await _apiService.RemoveAsync($"Cars/{id}");
            await _apiService.RemoveAsync($"CarDetails/{cars.CarDetailsId}");
            return RedirectToAction(nameof(Index));
        }
    }
}
