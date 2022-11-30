using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ModelBindingTask.Data;
using ModelBindingTask.Models;
using System.Configuration;
using System.Text;

internal class Program
{
        //private readonly Configuration _configuration;
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddIdentity<ModelBindingTask.Models.ApplicationUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = false;
        })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

        //builder.Services.AddDefaultIdentity<User>().AddEntityFrameworkStores<ApplicationDbContext>();
        //builder.Services.AddIdentity<IdentityUser,UserRoles>().AddEntityFrameworkStores<ApplicationDbContext>();
        //builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
        builder.Services.AddMvc(o => o.EnableEndpointRouting = false);

        //JWT
        // allows both to access and to set up the config

        builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/login";
        options.AccessDeniedPath = "/auth/forbidden";
        //options.LoginPath = "/auth/unauthorized";
        //options.AccessDeniedPath = "/auth/forbidden";

    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:44363/",
            ValidAudience = "https://localhost:44363/",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@1"))
        };
    })
    .AddJwtBearer("SecondJwtScheme", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:44363/",
            ValidAudience = "https://localhost:44363/",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@2"))
        };
    });

        builder.Services.AddAuthorization(options =>
        {
            var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                JwtBearerDefaults.AuthenticationScheme,
                CookieAuthenticationDefaults.AuthenticationScheme,
                "SecondJwtScheme");
            defaultAuthorizationPolicyBuilder =
                defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
            options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
        });

        //var jwtSection =Configuration.GetSection("JWTSettings");
        //builder.Services.Configure<JWTSettings>(jwtSection);

        var app = builder.Build();
        //IConfiguration configuration = app.Configuration;
        //IWebHostEnvironment environment = app.Environment;

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
        app.UseMvc();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}