using FirePlatform.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirePlatform.Models
{
    public class Context : DbContext
    {

        public DbSet<User> Users { get; set; }
        //public DbSet<UserRole> UserRoles { get; set; }
        //public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
#if DEBUG
                    optionsBuilder.UseSqlServer(@"Server=DESKTOP-5V6A2V8;Database=FirePlatformDB;Trusted_Connection=True;MultipleActiveResultSets=true;");
                    //optionsBuilder.UseSqlServer(@"Server=sql6001.site4now.net;Database=DB_A43487_FirePlatformDB;MultipleActiveResultSets=true;User Id=DB_A43487_FirePlatformDB_admin;Password=V@sya123!;");
#else
                optionsBuilder.UseSqlServer(@"Server=sql6001.site4now.net;Database=DB_A43487_FirePlatformDB;MultipleActiveResultSets=true;User Id=DB_A43487_FirePlatformDB_admin;Password=V@sya123!;");
#endif

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
        }
    }
}
