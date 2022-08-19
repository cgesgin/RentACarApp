using Microsoft.AspNetCore.Mvc;
using RentACar.Core.DTOs;

namespace RentACar.Web.MVC.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult Error(ErrorDto errorDto) 
        {
            return View(errorDto);
        }
    }
}
