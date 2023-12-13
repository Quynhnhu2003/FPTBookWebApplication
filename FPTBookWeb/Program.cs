using FPTBookWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FPTBookWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //Adding DBContext Service
            builder.Services.AddDbContext<DbFptbookContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

            builder.Services.AddDefaultIdentity<User>().AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DbFptbookContext>().AddDefaultUI();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.Configure<IdentityOptions>(option =>
            {
                option.Password.RequireNonAlphanumeric=false;
            });
            /*      builder.Services.AddScoped<Cart>(sp => Cart.GetCart(sp));

                  builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

                  builder.Services.AddRazorPages();

                  builder.Services.AddSession(option =>
                  {
                      option.Cookie.HttpOnly = true;
                      option.Cookie.IsEssential = true;
                      *//*option.IdleTimeout = TimeSpan.FromSeconds(10);*//*
                  });*/

            builder.Services.AddScoped<Cart>(sp => Cart.GetCart(sp));

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddRazorPages();

            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;

            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.MapRazorPages();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}