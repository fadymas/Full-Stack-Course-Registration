using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WithAuthintication;
using WithAuthintication.Models;

namespace WithAuthintication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
         
        }
        public DbSet<Client> clients { get; set; }
        public DbSet<Course> courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<ClientCourse> ClientCourses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<ApplicationUser>()
                .HasOne(a => a.Client)
                .WithOne(c => c.User)
                .HasForeignKey<Client>(c => c.UserId);
            // Use UserId instead of c.User.Id
            modelBuilder.Entity<ClientCourse>()
            .HasKey(cc => new { cc.ClientId, cc.CourseId });

            modelBuilder.Entity<ClientCourse>()
                .HasOne(cc => cc.Client)
                .WithMany(c => c.ClientCourses)
                .HasForeignKey(cc => cc.ClientId);

            modelBuilder.Entity<ClientCourse>()
                .HasOne(cc => cc.Course)
                .WithMany(c => c.ClientCourses)
                .HasForeignKey(cc => cc.CourseId);


            modelBuilder
                .Entity<Department>()
                .HasMany(d => d.Clients)
                .WithOne(c => c.Department)
                .HasForeignKey(c => c.DepartmentId);
            
            modelBuilder
                .Entity<Department>()
                .HasMany(d => d.Courses)
                .WithOne(c => c.Department)
                .HasForeignKey(c => c.DepartmentId);
        }
        public DbSet<WithAuthintication.Course> Course { get; set; } = default!;
    }
}
