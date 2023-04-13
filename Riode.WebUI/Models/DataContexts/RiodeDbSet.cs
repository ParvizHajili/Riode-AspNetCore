using Microsoft.AspNetCore.Identity;
using Riode.WebUI.Models.Membership;

namespace Riode.WebUI.Models.DataContexts
{
    static public class RiodeDbSet
    {
        static public IApplicationBuilder SeedMembership(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var role = new RiodeRole
                {
                    Name = "SuperAdmin"
                };

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<RiodeUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RiodeRole>>();

                if (roleManager.RoleExistsAsync(role.Name).Result)
                {
                    role = roleManager.FindByNameAsync(role.Name).Result;
                }
                else
                {
                    var roleCreateResult = roleManager.CreateAsync(role).Result;
                    if (!roleCreateResult.Succeeded)
                        goto end;
                }

                string pwd = "123";
                var user = new RiodeUser
                {
                    UserName = "Parviz",
                    Email = "parviz.hajili@gmail.com",
                    Name="Parviz",
                    SurName="Hajili",
                    EmailConfirmed = true
                };

                var foundedUser = userManager.FindByEmailAsync(user.Email).Result;

                if (foundedUser != null && !userManager.IsInRoleAsync(foundedUser, role.Name).Result)
                {
                    userManager.AddToRoleAsync(foundedUser, role.Name).Wait();
                }
                else if (foundedUser == null)
                {
                    var userCreateResult =userManager.CreateAsync(user,pwd).Result;
                    if(!userCreateResult.Succeeded)
                        goto end;

                    userManager.AddToRoleAsync(user,role.Name).Wait();
                }
            }
            end:
            return app;
        }
    }
}
