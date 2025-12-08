using Capstone_Project_PROG36944.Data;
using Capstone_Project_PROG36944.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog.Web;


namespace Capstone_Project_PROG36944
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 1) Configures NLog from nlog.config
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // 2) Uses NLog as logging provider
                builder.Logging.ClearProviders(); // remove default providers
                builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                builder.Host.UseNLog(); // add NLog

                // 3) Adds services to the container.
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString));

                builder.Services.AddDatabaseDeveloperPageExceptionFilter();

                builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
                        options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>();

                builder.Services.AddRazorPages();
                builder.Services.AddControllersWithViews();

                var app = builder.Build();

                // 4) Configures the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseMigrationsEndPoint();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                }

                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                app.MapRazorPages();

                app.Run();
            }
            catch (Exception ex)
            {
                // 5) Logs any fatal startup errors, if any
                logger.Error(ex, "Application stopped because of an unhandled exception during startup.");
                throw;
            }
            finally
            {
                // Ensures that logs are pushed out
                NLog.LogManager.Shutdown();
            }
        }
    }
}
