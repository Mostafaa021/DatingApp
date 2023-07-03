using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if(context.Users.Any())  return;    // if Database have any Data then Return
            string path = "Data/UserSeedData.json"; // Path of Json file 
            var userdata = await File.ReadAllTextAsync(path); // Read from The file in this path
            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true}; // make json options  case insensitive
            var users = JsonSerializer.Deserialize<List<AppUser>>(userdata,options); // convert from json to C# object ==> Deserlize
            foreach(var user in users){
                using var HMAC = new HMACSHA512();
                user.UserName = user.UserName.ToLower();
                user.PasswordHash = HMAC.ComputeHash(Encoding.UTF8.GetBytes("P@$$w0rd"));
                user.PasswordSalt = HMAC.Key ;
                context.Users.Add(user);
            }
            await context.SaveChangesAsync();

        }

    }
}