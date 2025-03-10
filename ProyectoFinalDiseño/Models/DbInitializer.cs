using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace ProyectoFinalDiseño.Models
{
    public class DbInitializer
    {


        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager)
        {

            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))

            {
                context.Database.EnsureCreated();


                if (context.Users.Any())
                {
                    return;
                }

                var user = new ApplicationUser
                {
                    UserName = "testuser@example.com",
                    Email = "testuser@example.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, "Test@1234");

                var admin = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, "Admin@1234");
            }
        
        }
    }
}
