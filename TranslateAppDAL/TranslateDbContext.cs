using System;
using Microsoft.EntityFrameworkCore;
using TranslateAppDAL.Domain;

namespace TranslateAppDAL
{
   public class TranslateDbContext : DbContext
   {
      public TranslateDbContext(DbContextOptions options) : base(options)
      {
      }

      protected override void OnModelCreating(ModelBuilder builder)
      {

         builder.Entity<Role>().HasData(new Role[]
            {new Role {Id = 1, Name = "admin"}, new Role {Id = 2, Name = "user"}});

         builder.Entity<User>().HasData(new User
         {
            Id = new Guid("C1EF5C4F-2D97-4045-AA1F-DA98620500CB"),
            Email = "georgevolkov@gmail.com",
            RoleId = 1,
            Password = "qwe123asd"
         });

         base.OnModelCreating(builder);
      }

      public DbSet<User> Users { get; set; }
      public DbSet<Role> Roles { get; set; }

      public DbSet<Translations> Translations { get; set; }
   }
}
