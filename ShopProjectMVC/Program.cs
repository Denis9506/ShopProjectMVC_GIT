
using Microsoft.EntityFrameworkCore;
using ShopProjectMVC.Core.Interfaces;
using ShopProjectMVC.Core.Services;
using ShopProjectMVC.Services;
using ShopProjectMVC.Storage;
using ShopProjectMVC.Storage.Repositories;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Local");

builder.Services.AddDbContext<ShopProjectContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IRepository, GenericRepository>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds((int)builder.Configuration.GetValue(typeof(int),"SessionTimeout"));
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true;
}
);


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Products}/{id?}");

app.Run();
