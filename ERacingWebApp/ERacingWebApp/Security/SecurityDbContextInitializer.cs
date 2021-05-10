using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

#region Additional Namespaces
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;
using System.Data.Entity;
using ERacingWebApp.Models;
using System.Collections.Specialized;
#endregion

namespace ERacingWebApp.Security
{
    public class SecurityDbContextInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {

        protected override void Seed(ApplicationDbContext context)
        {
            #region Phase A - Set up our Security Roles
            // 1. Instantiate a Controller class from ASP.Net Identity to add roles
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            // 2. Grab our list of security roles from the web.config
            var startupRoles = ConfigurationManager.AppSettings["startupRoles"].Split(';');
            // 3. Loop through and create the security roles
            foreach (var role in startupRoles)
                roleManager.Create(new IdentityRole { Name = role });
            #endregion

            #region Create List of Keys
            NameValueCollection users = GetUsers();
            #endregion

            #region Phase B - Add a Website Administrator
            // 1. Get the values from the <appSettings>
            string adminUser = ConfigurationManager.AppSettings["adminUserName"];
            string adminRole = ConfigurationManager.AppSettings["adminRole"];
            string adminEmail = ConfigurationManager.AppSettings["adminEmail"];
            string adminPassword = ConfigurationManager.AppSettings["adminPassword"];

            // 2. Instantiate my Controller to manage Users
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            //                \   IdentityConfig.cs    /             \IdentityModels.cs/
            // 3. Add the web admin to the database
            var result = userManager.Create(new ApplicationUser
            {
                UserName = adminUser,
                Email = adminEmail,
                EmployeeId = null
            }, adminPassword);
            if (result.Succeeded)
                userManager.AddToRole(userManager.FindByName(adminUser).Id, adminRole);
            #endregion

            #region Phase C - Add a Customer
            // 1. Get the values from the <appSettings>
            //int customerId = int.Parse(ConfigurationManager.AppSettings["customerId"]);
            //string customerUser = ConfigurationManager.AppSettings["customerUserName"];
            //string customerRole = ConfigurationManager.AppSettings["customerRole"];
            //string customerEmail = ConfigurationManager.AppSettings["customerEmail"];
            //string customerPassword = ConfigurationManager.AppSettings["customerPassword"];
            //result = userManager.Create(new ApplicationUser
            //{
            //    CustomerId = customerId,
            //    UserName = customerUser,
            //    Email = customerEmail,
            //    EmployeeId = null
            //}, customerPassword);
            //if (result.Succeeded)
            //    userManager.AddToRole(userManager.FindByName(customerUser).Id, customerRole);
            #endregion

            #region Phase D - Add a Employee
            // 1. Get the values from the <appSettings>
           
            
            int employeeId = int.Parse(ConfigurationManager.AppSettings["employeeId1"]);
            string employeeUser = ConfigurationManager.AppSettings["employeeUserName1"];
            string employeeRole = ConfigurationManager.AppSettings["employeeRole1"];
            string employeeEmail = ConfigurationManager.AppSettings["employeeEmail1"];
            string employeePassword = ConfigurationManager.AppSettings["employeePassword1"];
            result = userManager.Create(new ApplicationUser
            {
                EmployeeId = employeeId,
                UserName = employeeUser,
                Email = employeeEmail,
            }, employeePassword);
            if (result.Succeeded)
                userManager.AddToRole(userManager.FindByName(employeeUser).Id, employeeRole);

            employeeId = int.Parse(ConfigurationManager.AppSettings["employeeId2"]);
            employeeUser = ConfigurationManager.AppSettings["employeeUserName2"];
            employeeRole = ConfigurationManager.AppSettings["employeeRole2"];
            employeeEmail = ConfigurationManager.AppSettings["employeeEmail2"];
            employeePassword = ConfigurationManager.AppSettings["employeePassword2"];
            result = userManager.Create(new ApplicationUser
            {
                EmployeeId = employeeId,
                UserName = employeeUser,
                Email = employeeEmail,
            }, employeePassword);
            if (result.Succeeded)
                userManager.AddToRole(userManager.FindByName(employeeUser).Id, employeeRole);

            employeeId = int.Parse(ConfigurationManager.AppSettings["employeeId3"]);
            employeeUser = ConfigurationManager.AppSettings["employeeUserName3"];
            employeeRole = ConfigurationManager.AppSettings["employeeRole3"];
            employeeEmail = ConfigurationManager.AppSettings["employeeEmail3"];
            employeePassword = ConfigurationManager.AppSettings["employeePassword3"];
            result = userManager.Create(new ApplicationUser
            {
                EmployeeId = employeeId,
                UserName = employeeUser,
                Email = employeeEmail,
            }, employeePassword);
            if (result.Succeeded)
                userManager.AddToRole(userManager.FindByName(employeeUser).Id, employeeRole);

            employeeId = int.Parse(ConfigurationManager.AppSettings["employeeId4"]);
            employeeUser = ConfigurationManager.AppSettings["employeeUserName4"];
            employeeRole = ConfigurationManager.AppSettings["employeeRole4"];
            employeeEmail = ConfigurationManager.AppSettings["employeeEmail4"];
            employeePassword = ConfigurationManager.AppSettings["employeePassword4"];
            result = userManager.Create(new ApplicationUser
            {
                EmployeeId = employeeId,
                UserName = employeeUser,
                Email = employeeEmail,
            }, employeePassword);
            if (result.Succeeded)
                userManager.AddToRole(userManager.FindByName(employeeUser).Id, employeeRole);

            employeeId = int.Parse(ConfigurationManager.AppSettings["employeeId5"]);
            employeeUser = ConfigurationManager.AppSettings["employeeUserName5"];
            employeeRole = ConfigurationManager.AppSettings["employeeRole5"];
            employeeEmail = ConfigurationManager.AppSettings["employeeEmail5"];
            employeePassword = ConfigurationManager.AppSettings["employeePassword5"];
            result = userManager.Create(new ApplicationUser
            {
                EmployeeId = employeeId,
                UserName = employeeUser,
                Email = employeeEmail,
            }, employeePassword);
            if (result.Succeeded)
                userManager.AddToRole(userManager.FindByName(employeeUser).Id, employeeRole);

            employeeId = int.Parse(ConfigurationManager.AppSettings["employeeId6"]);
            employeeUser = ConfigurationManager.AppSettings["employeeUserName6"];
            employeeRole = ConfigurationManager.AppSettings["employeeRole6"];
            employeeEmail = ConfigurationManager.AppSettings["employeeEmail6"];
            employeePassword = ConfigurationManager.AppSettings["employeePassword6"];
            result = userManager.Create(new ApplicationUser
            {
                EmployeeId = employeeId,
                UserName = employeeUser,
                Email = employeeEmail,
            }, employeePassword);
            if (result.Succeeded)
                userManager.AddToRole(userManager.FindByName(employeeUser).Id, employeeRole);

            employeeId = int.Parse(ConfigurationManager.AppSettings["employeeId7"]);
            employeeUser = ConfigurationManager.AppSettings["employeeUserName7"];
            employeeRole = ConfigurationManager.AppSettings["employeeRole7"];
            employeeEmail = ConfigurationManager.AppSettings["employeeEmail7"];
            employeePassword = ConfigurationManager.AppSettings["employeePassword7"];
            result = userManager.Create(new ApplicationUser
            {
                EmployeeId = employeeId,
                UserName = employeeUser,
                Email = employeeEmail,
            }, employeePassword);
            if (result.Succeeded)
                userManager.AddToRole(userManager.FindByName(employeeUser).Id, employeeRole);

            employeeId = int.Parse(ConfigurationManager.AppSettings["employeeId8"]);
            employeeUser = ConfigurationManager.AppSettings["employeeUserName8"];
            employeeRole = ConfigurationManager.AppSettings["employeeRole8"];
            employeeEmail = ConfigurationManager.AppSettings["employeeEmail8"];
            employeePassword = ConfigurationManager.AppSettings["employeePassword8"];
            result = userManager.Create(new ApplicationUser
            {
                EmployeeId = employeeId,
                UserName = employeeUser,
                Email = employeeEmail,
            }, employeePassword);
            if (result.Succeeded)
                userManager.AddToRole(userManager.FindByName(employeeUser).Id, employeeRole);

            employeeId = int.Parse(ConfigurationManager.AppSettings["employeeId9"]);
            employeeUser = ConfigurationManager.AppSettings["employeeUserName9"];
            employeeRole = ConfigurationManager.AppSettings["employeeRole9"];
            employeeEmail = ConfigurationManager.AppSettings["employeeEmail9"];
            employeePassword = ConfigurationManager.AppSettings["employeePassword9"];
            result = userManager.Create(new ApplicationUser
            {
                EmployeeId = employeeId,
                UserName = employeeUser,
                Email = employeeEmail,
            }, employeePassword);
            if (result.Succeeded)
                userManager.AddToRole(userManager.FindByName(employeeUser).Id, employeeRole);

            employeeId = int.Parse(ConfigurationManager.AppSettings["employeeId10"]);
            employeeUser = ConfigurationManager.AppSettings["employeeUserName10"];
            employeeRole = ConfigurationManager.AppSettings["employeeRole10"];
            employeeEmail = ConfigurationManager.AppSettings["employeeEmail10"];
            employeePassword = ConfigurationManager.AppSettings["employeePassword10"];
            result = userManager.Create(new ApplicationUser
            {
                EmployeeId = employeeId,
                UserName = employeeUser,
                Email = employeeEmail,
            }, employeePassword);
            if (result.Succeeded)
                userManager.AddToRole(userManager.FindByName(employeeUser).Id, employeeRole);



            #endregion

            base.Seed(context);
        }

        private NameValueCollection GetUsers()
        {
            Object user;
            //List<Object> userList = new List<Object>();
            NameValueCollection userList = new NameValueCollection();
            //NameObjectCollectionBase userList = new NameObjectCollectionBase();
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                if (appSettings.Count == 0)
                {
                    Console.WriteLine("Appsettings is empty");
                }
                else
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        user = new Object();
                        if (key != "startupRoles" && key != "adminUserName" && key != "adminRole" && key != "adminEmail" && key != "adminPassword")
                        {
                            userList.Add(key, appSettings[key]);
                        }
                    }                    
                }               
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
            return userList;
        }
    }
}