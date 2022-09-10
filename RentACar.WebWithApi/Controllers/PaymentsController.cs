using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Core.DTOs;
using RentACar.WebWithApi.Service;

namespace RentACar.WebWithApi.Controllers
{
    [Authorize]
    public class PaymentsController : Controller
    {
        private readonly IApiService _apiService;

        public PaymentsController(IApiService apiService)
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
            var rental= await _apiService.GetByIdAsync<RentalDto>($"Rentals/{paymentDto.RentalId}");
            if (ModelState.IsValid)
            {
                await _apiService.SaveAsync<PaymentDto>("Payments", paymentDto);
                rental.Status = "paid";
                await _apiService.UpdateAsync<RentalDto>("Rentals", rental);
                return Redirect($"~/Rentals/Index");
            }
            ViewBag.rental = rental;
            return View();
        }

    }
}
