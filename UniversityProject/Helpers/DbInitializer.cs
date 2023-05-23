﻿using Microsoft.AspNetCore.Identity;
using UniversityProject.Constants;
using UniversityProject.Models;

namespace UniversityProject.Helpers
{
    public static class DbInitializer
    {
        public async static Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString()
                    });
                }
            }

            if((await userManager.FindByNameAsync("admin"))==null)  
            {
                var user = new User
                {
                    Fullname = "admin",
                    UserName = "admin",
                    Email = "admin@app.com"
                };
                var result = await userManager.CreateAsync(user, "Admin12345*");
                if (!result.Succeeded)
                {
                    foreach(var error in result.Errors)
                    {
                        throw new Exception(error.Description);
                    }
                }
                    await userManager.AddToRoleAsync(user, UserRoles.Admin.ToString());
            }
        }
    }
}
