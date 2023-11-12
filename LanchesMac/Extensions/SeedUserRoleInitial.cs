using LanchesMac.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace LanchesMac.Services
{
    public static class SeedUserRoleInitial
    {
        public static void ConfigurarBancoDados(this IServiceProvider services)
        {
            using var serviceScope = services.CreateScope();
            serviceScope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();

            SeedRoles(serviceScope);
            PrepararUsuarioInicial(serviceScope);
        }

        public static void SeedRoles(IServiceScope serviceScope)
        {
            var _roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!_roleManager.RoleExistsAsync("Member").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Member";
                role.NormalizedName = "MEMBER";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }
            if (!_roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                role.NormalizedName = "ADMIN";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }
        }
        public static void PrepararUsuarioInicial(IServiceScope serviceScope)
        {
            var _userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var usuarioInicial = _userManager.FindByEmailAsync("denis@gmail.com").GetAwaiter().GetResult();

            if (usuarioInicial is null)
            {
                IdentityUser user = new()
                {
                    UserName = "denis@gmail.com",
                    Email = "denis@gmail.com",
                    NormalizedUserName = "DENIS@GMAIL.COM",
                    NormalizedEmail = "DENIS@GMAIL.COM",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                IdentityResult result = _userManager.CreateAsync(user, "Denis@123456").Result;

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
