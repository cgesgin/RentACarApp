using Microsoft.AspNetCore.Mvc;
using RentACar.Core.DTOs;

namespace RentACar.API.Controllers
{

    public class CustomBaseController : ControllerBase
    {
        [NonAction]
        public IActionResult CreateActionResult<T>(ResponseDto<T> response)
        {
            if (response.StatusCode == 204)
                return new ObjectResult(null)
                {
                    StatusCode = response.StatusCode
                };

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };

        }
    }
}