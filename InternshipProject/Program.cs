using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace InternshipProject
{
    public class Program
    {
        
        public static void InitializeRoles(RoleManager<IdentityRole> roleManager)
        {
           var adminExists =  roleManager.RoleExistsAsync("Admin")
                       .GetAwaiter()
                       .GetResult();
            
            if (!adminExists)
            {
                roleManager.CreateAsync(new IdentityRole("Admin"))
                            .GetAwaiter()
                            .GetResult();
            }
            
        }

        public static void InitializeAdminUsers(UserManager<IdentityUser> userManager)
        {
            var adminUser = userManager.FindByEmailAsync("catalin.sbora@gmail.com")
                                        .GetAwaiter()
                                        .GetResult();
            if (adminUser != null)
            {
                var result = userManager.AddToRoleAsync(adminUser, "Admin")
                            .GetAwaiter()
                            .GetResult();
                if (!result.Succeeded)
                {
                    Log.Error("Failed to add Admin to user ....");
                }

            }
        }

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
              .Enrich.FromLogContext()
              .Enrich.WithThreadId()
              .Enrich.WithProcessId()
              .WriteTo.Console()
              .CreateLogger();
            IHost host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetService<RoleManager<IdentityRole>>();
                var userManager = services.GetService<UserManager<IdentityUser>>();
                InitializeRoles(roleManager);
                InitializeAdminUsers(userManager);

            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
