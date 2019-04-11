using CuttingSystem3mkMobile.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace CuttingSystem3mkMobile.Models
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<CutModel> CutModel { get; set; }
        public DbSet<CutCode> CuttingCode { get; set; }
        public DbSet<CutFile> CutFile { get; set; }
        public DbSet<DeviceModel> DeviceModel { get; set; }
        //public DbSet<UserRole> UserRoles { get; set; }
        //public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                   // optionsBuilder.UseSqlServer(@"Server=DESKTOP-5V6A2V8;Database=CuttingSystem;Trusted_Connection=True;MultipleActiveResultSets=true;");
                    optionsBuilder.UseSqlServer(@"Server=sql6006.site4now.net;Database=DB_A47482_CuttingSystem;MultipleActiveResultSets=true;User Id=DB_A47482_CuttingSystem_admin;Password=V@sya123!;");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<CutCode>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<CutCode>()
               .HasOne(e => e.CutModel)
               .WithMany(f => f.CutCodes);

            modelBuilder.Entity<CutModel>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<CutModel>()
                .HasOne(e => e.CutFile);

            modelBuilder.Entity<CutModel>()
                .HasOne(e => e.DeviceModel)
                .WithMany(f => f.CutModels);

            modelBuilder.Entity<CutModel>()
                .HasMany(e => e.CutCodes)
                .WithOne(f => f.CutModel);

            modelBuilder.Entity<DeviceModel>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<DeviceModel>()
                .HasMany(e => e.CutModels)
                .WithOne(f => f.DeviceModel);

            modelBuilder.Entity<CutFile>()
                .HasKey(u => u.Id);
        }
    }
}
