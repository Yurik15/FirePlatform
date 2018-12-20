using FirePlatform.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace FirePlatform.Models
{
    public class Context : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<TemplateModel> Forms { get; set; }
        public DbSet<UserForm> UserForms { get; set; }
        //public DbSet<UserRole> UserRoles { get; set; }
        //public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
#if DEBUG
                    //optionsBuilder.UseSqlServer(@"Server=DESKTOP-5V6A2V8;Database=FirePlatformDB;Trusted_Connection=True;MultipleActiveResultSets=true;");
                    optionsBuilder.UseSqlServer(@"Server=sql6001.site4now.net;Database=DB_A43487_FirePlatformDB;MultipleActiveResultSets=true;User Id=DB_A43487_FirePlatformDB_admin;Password=V@sya123!;");
#else
                optionsBuilder.UseSqlServer(@"Server=sql6001.site4now.net;Database=DB_A43487_FirePlatformDB;MultipleActiveResultSets=true;User Id=DB_A43487_FirePlatformDB_admin;Password=V@sya123!;");
#endif

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<TemplateModel>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserForm>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserForm>()
                .HasKey(us => new { us.UserId, us.FormId });

            modelBuilder.Entity<UserForm>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserForms)
                .HasForeignKey(us => us.UserId);

            modelBuilder.Entity<UserForm>()
                .HasOne(us => us.Form)
                .WithMany(f => f.UserForms)
                .HasForeignKey(us => us.FormId);
        }
    }
}
