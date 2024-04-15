using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager,   // instead of injecting DataContext
                         RoleManager<AppRole> roleManager) 
        {
            if(await userManager.Users.AnyAsync())  return;    // if Database have any Data then Return
            string path = "Data/UserSeedData.json"; // Path of Json file 
            var userdata = await File.ReadAllTextAsync(path); // Read from The file in this path
            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true}; // make json options  case insensitive
            var users = JsonSerializer.Deserialize<List<AppUser>>(userdata,options); // convert from json to C# object ==> Deserlize

            // create roles needed for our application 
            var roles = new List<AppRole>()
            {
                new AppRole { Name = "Member"} ,
                new AppRole { Name = "Admin"} ,
                new AppRole { Name = "Moderator"} ,

            };

            // for each role user Service from Identity to save this role to database 
            foreach(var role in roles)
            {
                await roleManager.CreateAsync(role); // will create and save changes in database 
            }
            foreach (var user in users){
                //using var HMAC = new HMACSHA512();
                //user.PasswordHash = HMAC.ComputeHash(Encoding.UTF8.GetBytes("P@$$w0rd"));
                //user.PasswordSalt = HMAC.Key ;

                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user, "P@$$w0rd"); // will create and save changes in database 
                await userManager.AddToRoleAsync(user, "Member"); // for each user give this role 
            }
            // create a new user with name  admin 
            var admin = new AppUser
            {
                UserName = "Admin"
            };
            // save this user  as admin with it`s password 
            await userManager.CreateAsync(admin , "P@$$w0rd");
            // add new roles to this user 
            await userManager.AddToRolesAsync(admin, new List<string>() { "Admin", "Moderator" });
        }

    }
}