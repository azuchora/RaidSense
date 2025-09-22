using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RaidSense.Server.Constants;
using RaidSense.Server.Data;
using RaidSense.Server.Models;

namespace RaidSense.Server.Extensions
{
    public static class DataExtensions
    {
        public static async Task SeedRolesAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

            foreach (var roleName in Roles.All)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            if (env.IsDevelopment())
            {
                var adminEmail = "admin@rustsense.xd";
                var admin = await userManager.FindByEmailAsync(adminEmail);
                if (admin != null) return;

                admin = new User
                {
                    UserName = "Admin",
                    Email = adminEmail,
                };

                await userManager.CreateAsync(admin, "Admin1@3");
                await userManager.AddToRoleAsync(admin, Roles.Admin);
            }
        }

        public static async Task MigrateDbAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.MigrateAsync();
        }

        public static void AddDatabaseAndIdentity(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<AppDbContext>();
        }
    }
}
