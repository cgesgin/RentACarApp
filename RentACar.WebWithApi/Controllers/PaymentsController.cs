using Microsoft.AspNetCore.Mvc;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Service;

namespace RentACar.WebWithApi.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ApiService _apiService;

        public PaymentsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Save(int id)
        {
            var rental = await _apiService.GetByIdAsync<RentalDto>($"Rentals/{id}");
            ViewBag.rental = rental;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(PaymentDto paymentDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.SaveAsync<PaymentDto>("Payments", paymentDto);
                return Redirect($"~/Rentals/Index");
            }
            ViewBag.rental = await _apiService.GetByIdAsync<RentalDto>($"Rentals/{paymentDto.RentalId}");
            return View();
        }

    }
}
