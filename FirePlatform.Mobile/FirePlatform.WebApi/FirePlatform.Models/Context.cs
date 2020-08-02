using FirePlatform.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace FirePlatform.Models
{
    public class Context : DbContext
    {

        public DbSet<Users> Users { get; set; }
        public DbSet<TemplateModel> Forms { get; set; }
        public DbSet<UserForm> UserForms { get; set; }
        public DbSet<UserTemplates> UserTemplates { get; set; }
        public DbSet<MainTemplates> MainTemplates { get; set; }
        public DbSet<ScriptDefinition> ScriptDefinitions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
#if DEBUG
                optionsBuilder.UseSqlServer(@"Server=SQL6007.site4now.net;Database=DB_A4DBB1_shine15;User Id = DB_A4DBB1_shine15_admin; Password = yB2Fh8SE6kF9NT5v;");
                //optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\GitHub\FirePlatform\FirePlatform.Mobile\FirePlatform.WebApi\FirePlatform.Models\FireplatformDB.mdf;Integrated Security=True");
#else
                optionsBuilder.UseSqlServer(@"Server=SQL6007.site4now.net;Database=DB_A4DBB1_shine15;User Id = DB_A4DBB1_shine15_admin; Password = yB2Fh8SE6kF9NT5v;");
#endif

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<MainTemplates>()
               .HasKey(u => u.Id);

            modelBuilder.Entity<TemplateModel>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserTemplates>()
              .HasOne(us => us.User)
               .WithMany(f => f.UserTemplates)
               .HasForeignKey(us => us.UserId);

            modelBuilder.Entity<Users>()
              .HasMany(t => t.UserTemplates)
               .WithOne(u => u.User)
               .HasForeignKey(us => us.UserId);

            modelBuilder.Entity<ScriptDefinition>()
               .HasKey(u => u.Id);
        }
    }
}
