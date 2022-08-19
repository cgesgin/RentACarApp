using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RentACar.Core.DTOs;
using RentACar.Core.Models;
using RentACar.Core.Services;

namespace RentACar.Web.MVC.Filters
{
    public class NotFoundFilter<T>:IAsyncActionFilter where T :BaseEntity
    {
        private readonly IService<T> _service;

        public NotFoundFilter(IService<T> service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            if (context.ActionArguments.ContainsKey("id"))
            {
                var id = context.ActionArguments["id"] as int?;
                if (id.HasValue)
                {
                    var anyEntity = await _service.AnyAsync(x => x.Id == id);
                    if (!anyEntity)
                    {
                        var errorDto = new ErrorDto();
                        errorDto.Errors.Add($"{typeof(T).Name}({id}) not found");
                        context.Result = new RedirectToActionResult("Error","Errors",errorDto);
                        return;
                    }
                }

            }
            await next();
        }
    }
}
