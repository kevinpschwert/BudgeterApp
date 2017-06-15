namespace BudgetApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BudgetApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }        

        protected override void Seed(BudgetApp.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(context));
            //Admin

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            var userManager = new UserManager<ApplicationUser>(
             new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "kevinpschwert@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "kevinpschwert@gmail.com",
                    Email = "kevinpschwert@gmail.com",
                    FirstName = "Kevin",
                    LastName = "Schwert",
                    DisplayName = "kschwert",
                    //HouseHoldId = 1,
                    EmailConfirmed = true
                }, "1th@nkGod!");
            }

            var userId = userManager.FindByEmail("kevinpschwert@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");

            //Guest PM

            if (!context.Roles.Any(r => r.Name == "Demo"))
            {
                roleManager.Create(new IdentityRole { Name = "Demo" });
            }

            if (!context.Users.Any(u => u.Email == "budgeterguest@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "budgeterguest@mailinator.com",
                    Email = "budgeterguest@mailinator.com",
                    FirstName = "Guest",
                    LastName = "Budgeter",
                    DisplayName = "Demo",
                    HouseHoldId = 6,
                    EmailConfirmed = true
                }, "Abc123!");
            }

            var userId5 = userManager.FindByEmail("budgeterguest@mailinator.com").Id;
            userManager.AddToRole(userId5, "Demo");

        }
    }
}
