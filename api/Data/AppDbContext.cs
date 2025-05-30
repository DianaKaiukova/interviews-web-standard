using Microsoft.EntityFrameworkCore;
using api.Models;

namespace api.Data
{
    // This class represents the application's database context, which is used to interact with the database.
    public class AppDbContext : DbContext
    {
        // Constructor that accepts DbContextOptions and passes them to the base class.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<api.Models.Task> Tasks => Set<api.Models.Task>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<TaskTag> TaskTags => Set<TaskTag>();

        // This method is called by the runtime to configure the model and relationships.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure many-to-many relationship
            modelBuilder.Entity<TaskTag>()
                .HasKey(tt => new { tt.TaskId, tt.TagId });

            modelBuilder.Entity<TaskTag>()
                .HasOne(tt => tt.Task)
                .WithMany(t => t.TaskTags)
                .HasForeignKey(tt => tt.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskTag>()
                .HasOne(tt => tt.Tag)
                .WithMany(t => t.TaskTags)
                .HasForeignKey(tt => tt.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}