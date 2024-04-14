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
        public static async Task SeedUsers(UserManager<AppUser> userManager) // instead of injecting DataContext
        {
            if(await userManager.Users.AnyAsync())  return;    // if Database have any Data then Return
            string path = "Data/UserSeedData.json"; // Path of Json file 
            var userdata = await File.ReadAllTextAsync(path); // Read from The file in this path
            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true}; // make json options  case insensitive
            var users = JsonSerializer.Deserialize<List<AppUser>>(userdata,options); // convert from json to C# object ==> Deserlize
            foreach(var user in users){
                //using var HMAC = new HMACSHA512();
                //user.PasswordHash = HMAC.ComputeHash(Encoding.UTF8.GetBytes("P@$$w0rd"));
                //user.PasswordSalt = HMAC.Key ;

                user.UserName = user.UserName.ToLower();
                 await userManager.CreateAsync(user, "P@$$w0rd"); // will create and save changes in database 
            }
            

        }

    }
}