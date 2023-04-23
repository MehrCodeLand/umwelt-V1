using Microsoft.EntityFrameworkCore;
using umweltV1.Core.Interfaces;
using umweltV1.Core.Repositories;
using umweltV1.Data.MyDbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MyDb>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("SqlStr")
    ));
builder.Services.AddScoped<IMainService, MainRepository>();
builder.Services.AddScoped<IAdminService, AdminRepository>();

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

app.UseAuthorization();



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
  name: "Admin",
  pattern: "{area:exists}/{controller=Home}/{action=AdminHome}/{id?}"
    );
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});



app.MapControllerRoute(
    name: "default",
    pattern: "{area=Admin}/{controller=Home}/{action=AdminHome}/{id?}");

app.Run();
