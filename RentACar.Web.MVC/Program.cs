using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using RentACar.Redis;
using RentACar.Repository;
using RentACar.Service.Mapping;
using RentACar.Service.Validations;
using RentACar.Web.MVC.Modules;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Add FluentValidation
builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CostumerDtoValidator>());

//Add InMemoryCache
builder.Services.AddMemoryCache();

//Add Redis
builder.Services.AddSingleton<RedisConnectionService>();

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
var redisService = app.Services.GetService<RedisConnectionService>();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

redisService.Connect();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
