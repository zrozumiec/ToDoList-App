using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Infrastructure
{
    /// <summary>
    /// Application DbContext.
    /// </summary>
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by a DbContext.</param>
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets task categories.
        /// </summary>
        public DbSet<TaskCategory> TaskCategories { get; set; }

        /// <summary>
        /// Gets or sets task notes.
        /// </summary>
        public DbSet<TaskNotes> TaskNotes { get; set; }

        /// <summary>
        /// Gets or sets task priorities.
        /// </summary>
        public DbSet<TaskPriority> TaskPriorities { get; set; }

        /// <summary>
        /// Gets or sets task statuses.
        /// </summary>
        public DbSet<TaskStatuses> TaskStatuses { get; set; }

        /// <summary>
        /// Gets or sets ToDoLists.
        /// </summary>
        public DbSet<ToDoList> ToDoLists { get; set; }

        /// <summary>
        /// Gets or sets ToDoTasks.
        /// </summary>
        public DbSet<ToDoTask> ToDoTasks { get; set; }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder?.Entity<TaskCategory>()
                .HasData(
                new TaskCategory()
                {
                    Id = 1,
                    Name = "Blue",
                    Description = "Blue category",
                },
                new TaskCategory()
                {
                    Id = 2,
                    Name = "Green",
                    Description = "Green category",
                },
                new TaskCategory()
                {
                    Id = 3,
                    Name = "Red",
                    Description = "Red category",
                });

            builder?.Entity<TaskPriority>()
                .HasData(
                new TaskPriority()
                {
                    Id = 1,
                    Name = "Low",
                },
                new TaskPriority()
                {
                    Id = 2,
                    Name = "Medium",
                },
                new TaskPriority()
                {
                    Id = 3,
                    Name = "High",
                });

            builder?.Entity<TaskStatuses>()
                .HasData(
                new TaskStatuses()
                {
                    Id = 1,
                    Name = "Not Started",
                    Description = "Task not started yet.",
                },
                new TaskStatuses()
                {
                    Id = 2,
                    Name = "In progress",
                    Description = "Task already started but not completed.",
                },
                new TaskStatuses()
                {
                    Id = 3,
                    Name = "Completed",
                    Description = "Task completed.",
                });
        }
    }
}
