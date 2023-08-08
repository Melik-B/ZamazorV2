using AppCore.DataAccess.Configs;
using Business.Services.Bases;
using Business.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using System.ComponentModel.Design;
using System.Globalization;
using Zamazor.Settings;

var builder = WebApplication.CreateBuilder(args);

List<CultureInfo> _cultures = new List<CultureInfo>()
{
    new CultureInfo("en-EN")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(_cultures.FirstOrDefault().Name);
    options.SupportedCultures = _cultures;
    options.SupportedUICultures = _cultures;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(config =>
    {
        config.LoginPath = "/Accounts/Login";
        config.AccessDeniedPath = "/Accounts/YetkisizIslem";
        config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        config.SlidingExpiration = true;
    });
#endregion

#region Session
builder.Services.AddSession(config =>
{
    config.IdleTimeout = TimeSpan.FromMinutes(30); //20 dakika
});
#endregion

ConnectionConfig.ConnectionString = builder.Configuration.GetConnectionString("ZamazorContext");

IConfigurationSection section = builder.Configuration.GetSection(nameof(AppSettings));
section.Bind(new AppSettings());

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IStoreService, StoreService>();

var app = builder.Build();

app.UseRequestLocalization(new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture(_cultures.FirstOrDefault().Name),
    SupportedCultures = _cultures,
    SupportedUICultures = _cultures,
});

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

#region Authentication
app.UseAuthentication();
#endregion

app.UseAuthorization();

#region Session
app.UseSession();
#endregion

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();