using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ProyectoFinalDiseño.Models.user;

namespace ProyectoFinalDiseño.Data
{
    public static class RoleInitializer
    {
        public static async Task InitializeAsync(
    RoleManager<IdentityRole> roleManager,
    UserManager<User_Application> userManager,
    ILogger logger) // Adding logger to capture logs
        {
              

            string[] roles = { "Admin", "Trainer", "Client", "Default" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!roleResult.Succeeded)
                    {
                        logger.LogError($"Error creating role: {role}");
                    }
                }
            }

            string adminEmail = "admin@example.com";
            string adminPassword = "Admin123!";
           
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new User_Application
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "Admin", // Ensure you set default values for custom properties
                    Lastname = "Admin", // Same here
                    ProfilePicture = "string" // Default value for profile picture
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    logger.LogError("Error creating admin user.");
                }
            }
             
        }

    }
}
