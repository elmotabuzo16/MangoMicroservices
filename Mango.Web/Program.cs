using Mango.Web.Service;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// The service below is used for IHttpClientFactory from BaseService.cs
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<ICouponService, CouponService>(); // Configure that coupon service will be using HttpClient
builder.Services.AddHttpClient<IAuthService, AuthService>(); // Configure that auth service will be using HttpClient
builder.Services.AddHttpClient<IProductService, ProductService>(); // Configure that product service will be using HttpClient
builder.Services.AddHttpClient<ICartService, CartService>(); // Configure that cart service will be using HttpClient

StaticDetails.CouponAPIBaseURL = builder.Configuration["ServiceUrls:CouponAPI"];
StaticDetails.AuthAPIBaseURL = builder.Configuration["ServiceUrls:AuthAPI"];
StaticDetails.ProductAPIBaseURL = builder.Configuration["ServiceUrls:ProductAPI"];
StaticDetails.ProductAPIBaseURL = builder.Configuration["ServiceUrls:ProductAPI"];
StaticDetails.ShoppingCartAPIBaseURL = builder.Configuration["ServiceUrls:ShoppingCartAPI"];

builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.LoginPath = "/auth/login";
        options.AccessDeniedPath = "/auth/accessdenied";
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
