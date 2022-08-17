using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RentACar.Core.DTOs;
using RentACar.Core.Models;
using RentACar.Core.Services;

namespace RentACar.API.Filters
{
    public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity
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
                        context.Result = new NotFoundObjectResult(ResponseDto<NoContentDto>.Fail(404, $"{typeof(T).Name}({id}) filter not found"));
                        return;
                    }
                }

            }
            await next();
        }
    }
}
