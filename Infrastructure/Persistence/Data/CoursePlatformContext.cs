using APICoursePlatform.Models;
using Domain.Common;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Data
{
    public class CoursePlatformContext : IdentityDbContext<ApplicationUser>
    {
        public CoursePlatformContext(DbContextOptions<CoursePlatformContext> options) : base(options) { }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<CreditDebitCardPaymentMethod> CreditCardPaymentMethods { get; set; }
        public DbSet<PayPalPaymentMethod> PayPalPaymentMethods { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PaymentCourse> PaymentCourses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // إعداد التوارث (inheritance) بين PaymentMethod و الـ subclasses
            modelBuilder.Entity<PaymentMethod>()
                .UseTptMappingStrategy();
            // ممكن تستخدم TPC أو TPH حسب ما تفضليه؛ TPT يخلي كل subclass عنده جدول خاص
            // EF Core docs بيشرح الاستراتيجيات دي :contentReference[oaicite:0]{index=0}

            modelBuilder.Entity<CreditDebitCardPaymentMethod>().ToTable("CreditCardPaymentMethods");
            modelBuilder.Entity<PayPalPaymentMethod>().ToTable("PayPalPaymentMethods");

            // علاقة Payment مع PaymentMethod
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.PaymentMethod)
                .WithMany()
                .HasForeignKey(p => p.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            // علاقة Many-to-Many من Payment → Course عبر PaymentCourse
            modelBuilder.Entity<PaymentCourse>()
                .HasOne(pc => pc.Payment)
                .WithMany(p => p.PaymentCourses)
                .HasForeignKey(pc => pc.PaymentId);

            modelBuilder.Entity<PaymentCourse>()
                .HasOne(pc => pc.Course)
                .WithMany(c => c.PaymentCourses)
                .HasForeignKey(pc => pc.CourseId);

            // علاقة CartItem
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(ci => ci.UserId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Course)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CourseId);

            // علاقات Course مع Category و Instructor و Lecture
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Category)
                .WithMany(cat => cat.Courses)
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorId);

            modelBuilder.Entity<Lecture>()
                .HasOne(l => l.Course)
                .WithMany(c => c.Lectures)
                .HasForeignKey(l => l.CourseId);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is IBaseModel &&
                            (e.State == EntityState.Added ||
                             e.State == EntityState.Modified ||
                             e.State == EntityState.Deleted));

            foreach (var entry in entries)
            {
                var entity = (IBaseModel)entry.Entity;

                

                if (entry.State == EntityState.Deleted)
                {
                    // Soft delete
                    entry.State = EntityState.Modified;
                    entity.IsDeleted = true;
                    
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
