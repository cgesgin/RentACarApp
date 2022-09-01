using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentACar.Repository;
using RentACar.Service.Validations;
using RentACar.WebWithApi.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CostumerDtoValidator>());

//HttpClient
builder.Services.AddHttpClient<IApiService,ApiService> (option =>
{
    option.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
});

var SqlCon = builder.Configuration.GetConnectionString("SqlCon");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(SqlCon, ServerVersion.AutoDetect(SqlCon)));

//Login Services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
     .AddDefaultTokenProviders().AddDefaultUI()
     .AddEntityFrameworkStores<AppDbContext>();
//Login Services
builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Users/Login";
    config.AccessDeniedPath = new PathString("/Users/AccessDenied");
});

var app = builder.Build();

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


app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
