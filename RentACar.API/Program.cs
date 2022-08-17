using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACar.API.Filters;
using RentACar.API.Middlewares;
using RentACar.API.Modules;
using RentACar.Core.Repositories;
using RentACar.Core.Services;
using RentACar.Core.UnitOfWorks;
using RentACar.Repository;
using RentACar.Repository.Repositories;
using RentACar.Repository.UnitOfWorks;
using RentACar.Service.Mapping;
using RentACar.Service.Services;
using RentACar.Service.Validations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add InMemoryCache
builder.Services.AddMemoryCache();

// Add Filter
builder.Services.AddScoped(typeof(NotFoundFilter<>));

//Add Validation

builder.Services.AddControllers(options => { options.Filters.Add(new ValidateFilterAttribute()); }).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CostumerDtoValidator>());
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;

});
//Add Project DI 
builder.Services.AddAutoMapper(typeof(MapProfile));


//Connection Database
var SqlCon = builder.Configuration.GetConnectionString("SqlCon");
builder.Services.AddDbContext<AppDbContext>(
        options => options.UseMySql(SqlCon,
        ServerVersion.AutoDetect(SqlCon), x =>
        {
            //x.MigrationsAssembly("RentACar.Repository");
            x.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
        })
    );
//Autofac Ioc container
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Global Costum Exceptionhendlar
app.UseCustomException();

app.UseAuthorization();

app.MapControllers();

app.Run();
