using Microsoft.EntityFrameworkCore;
using RentACar.Core.Repositories;
using RentACar.Core.Services;
using RentACar.Core.UnitOfWorks;
using RentACar.Repository;
using RentACar.Repository.Repositories;
using RentACar.Repository.UnitOfWorks;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IUnitOfWork,UnitOfWorkImp>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositoryImp<>));
//builder.Services.AddScoped(typeof(IService<>), typeof(ServiceImp<>);

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
