using CRM.Domain.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Identity;

public class SeedUserRoleInitial : ISeedUserRoleInitial
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public SeedUserRoleInitial(RoleManager<IdentityRole> roleManager,
          UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public void SeedUsers()
    {
        if (_userManager.FindByEmailAsync("usuario@localhost").Result == null)
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = "usuario@localhost";
            user.Email = "usuario@localhost";
            user.NormalizedUserName = "USUARIO@LOCALHOST";
            user.NormalizedEmail = "USUARIO@LOCALHOST";
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            user.SecurityStamp = Guid.NewGuid().ToString();

            IdentityResult result = _userManager.CreateAsync(user, "Numsey#2021").Result;

            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "User").Wait();
            }
        }

        if (_userManager.FindByEmailAsync("admin@localhost").Result == null)
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = "admin@localhost";
            user.Email = "admin@localhost";
            user.NormalizedUserName = "ADMIN@LOCALHOST";
            user.NormalizedEmail = "ADMIN@LOCALHOST";
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            user.SecurityStamp = Guid.NewGuid().ToString();

            IdentityResult result = _userManager.CreateAsync(user, "Numsey#2021").Result;

            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, "Admin").Wait();
            }
        }

    }

    public void SeedRoles()
    {
        if (!_roleManager.RoleExistsAsync("User").Result)
        {
            IdentityRole role = new IdentityRole();
            role.Name = "User";
            role.NormalizedName = "USER";
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
}
