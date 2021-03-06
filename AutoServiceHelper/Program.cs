using AutoServiceHelper.Core.Contracts;
using AutoServiceHelper.Core.Services;
using AutoServiceHelper.Infrastructure.Data;
using AutoServiceHelper.Infrastructure.Data.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews(options =>
{ 
    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
}) ;

builder.Services.AddTransient<ICarService, CarServices>();
builder.Services.AddTransient<IAutoShopServices, AutoshopServices>();
builder.Services.AddTransient<IInformationServices, InformationServices>();
builder.Services.AddTransient<IMechanicServices, MechanicServices>();
builder.Services.AddTransient<IManagerServices, ManagerServices>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRepository, Repository>();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric=false;
    options.Password.RequiredUniqueChars=0;
    options.Password.RequireDigit=false;
    options.Password.RequireUppercase=false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
