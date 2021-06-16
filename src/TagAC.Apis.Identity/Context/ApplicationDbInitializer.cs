using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace TagAC.Apis.Identity.Context
{
    public class ApplicationDbInitializer
    {
        public static async Task SeedAdminUser(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminUser = await userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(user, "123@qwe");

                if (result.Succeeded)
                {
                    var roleExists = await roleManager.RoleExistsAsync("Admin");
                    if (!roleExists)
                    {
                        await roleManager.CreateAsync(new IdentityRole()
                        {
                            Name = "Admin",
                            NormalizedName = "Admin"
                        });
                    }

                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
