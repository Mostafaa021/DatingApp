using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Data
{
    public class DataContext : IdentityDbContext <AppUser , AppRole , int, 
            IdentityUserClaim<int> , AppUserRole
           ,IdentityUserLogin<int> , IdentityRoleClaim<int> , IdentityUserToken<int> >
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        //public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes {get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<AppUser>(b =>
            {
                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
                // Each User can have many UserClaims
                //b.HasMany(e => e.Claims)
                //    .WithOne()
                //    .HasForeignKey(uc => uc.UserId)
                //    .IsRequired();

                //// Each User can have many UserLogins
                //b.HasMany(e => e.Logins)
                //    .WithOne()
                //    .HasForeignKey(ul => ul.UserId)
                //    .IsRequired();

                //// Each User can have many UserTokens
                //b.HasMany(e => e.Tokens)
                //    .WithOne()
                //    .HasForeignKey(ut => ut.UserId)
                //    .IsRequired();

            });

            modelBuilder.Entity<AppRole>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
                // Each User can have many RoleClaims
                //b.HasMany(e => e.RoleClaims)
                //    .WithOne()
                //    .HasForeignKey(uc => uc.RoleId)
                //    .IsRequired();
            });


            modelBuilder.Entity<UserLike>()
            .HasKey(k=>new{k.SourceUserId,k.TargetUserId});

            modelBuilder.Entity<UserLike>()
            .HasOne(s=>s.SourceUser)
            .WithMany(l=>l.LikedUsers)
            .HasForeignKey(s=>s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserLike>()
            .HasOne(s=>s.TargetUser)
            .WithMany(l=>l.LikedByUsers)
            .HasForeignKey(s=>s.TargetUserId)
            .OnDelete(DeleteBehavior.Cascade); // if you are  use SQL Server should one of deletingBehaviour be different 

            modelBuilder.Entity<Message>()
            .HasOne(u => u.Recipient)
            .WithMany(m => m.MessagesRecieved)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
            .HasOne(u => u.Sender)
            .WithMany(m => m.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);
        }

    }
}