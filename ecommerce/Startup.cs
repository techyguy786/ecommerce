using System;
using ecommerce.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Configuration;

[assembly: OwinStartupAttribute(typeof(ecommerce.Startup))]

namespace ecommerce
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRoleAndUser();
        }

        private void CreateRoleAndUser()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // In Startup iam creating first Admin Role and creating a default Admin User 
            if (!roleManager.RoleExists("Admin"))
            {
                // first we create Admin rool
                var role = new IdentityRole { Name = "Admin" };
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website				

                var user = new ApplicationUser
                {
                    Name = ConfigurationManager.AppSettings["admin"],
                    UserName = ConfigurationManager.AppSettings["adminUserName"],
                    Email = ConfigurationManager.AppSettings["adminUserName"]
                };

                string userPWD = ConfigurationManager.AppSettings["adminPassword"];

                var chkUser = userManager.Create(user, userPWD);

                if (!chkUser.Succeeded)
                {
                    throw new ApplicationException("Unable to create a user.");
                }

                var result1 = userManager.AddToRole(user.Id, "Admin");
                if (!result1.Succeeded)
                {
                    throw new ApplicationException("Unable to add user to a role.");
                }
            }
        }
    }
}
