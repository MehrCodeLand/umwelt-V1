using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using umweltV1.Core.Interfaces;
using umweltV1.Core.Repositories;
using umweltV1.Data.MyDbContext;
using static umweltV1.Security.Sender.EmailSender.RenderFile.ViewToString;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// login and identity
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/SignIn";
    options.LogoutPath = "/SignOut";
    options.ExpireTimeSpan = TimeSpan.FromDays(15);
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 5;
});
builder.Services.Configure<SecurityStampValidatorOptions>(x =>
{
    x.ValidationInterval = TimeSpan.Zero;
});


builder.Services.AddDbContext<MyDb>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("SqlStr")
    ));
builder.Services.AddScoped<IMainService, MainRepository>();
builder.Services.AddScoped<IAdminService, AdminRepository>();
builder.Services.AddScoped<IViewRenderService, RenderViewToString>();

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
app.UseAuthentication();


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
