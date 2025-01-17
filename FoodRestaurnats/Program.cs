using FoodRestaurnats.Data;
using FoodRestaurnats.Data.interfaces;
using FoodRestaurnats.Data.mocks;
using FoodRestaurnats.Data.Models;
using FoodRestaurnats.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IfoodRepository, foodRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped(sp => ShoppingCart.GetCart(sp));
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddMvc(option => option.EnableEndpointRouting = false);
builder.Services.AddMemoryCache();
builder.Services.AddSession();

builder.Services.AddDbContext<AppDbContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
builder.Logging.AddConsole();

app.UseDeveloperExceptionPage();
app.UseStatusCodePages();
app.UseStaticFiles();
app.UseSession();
DbInitializer.Seed(app);
//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();
app.UseMvc(routes =>
{
    routes.MapRoute(name: "CategoryFilter", template: "food/{action}/{category?}", defaults: new { controller = "food", action = "List" });
    routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");

});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
