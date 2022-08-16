using Microsoft.AspNetCore.Diagnostics;
using RentACar.Core.DTOs;
using RentACar.Service.Exceptions;
using System.Text.Json;

namespace RentACar.API.Middlewares
{
    public static class UseCustomExceptionHandler
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config => 
            {
                config.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var statusCode = exceptionFeature.Error switch
                    {
                        ClientSideException => 400,
                        NotFoundExcepiton => 404,
                        _ => 500
                    };
                    context.Response.StatusCode = statusCode;
                    var response = ResponseDto<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                });
            });
        }
    }
}
