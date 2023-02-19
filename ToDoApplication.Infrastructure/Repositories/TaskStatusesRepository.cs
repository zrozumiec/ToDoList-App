using Microsoft.EntityFrameworkCore;
using ToDoApplication.Domain.Interfaces;
using ToDoApplication.Domain.Models;
using ToDoApplication.Infrastructure.Repositories.Base;

namespace ToDoApplication.Infrastructure.Repositories
{
    /// <summary>
    /// Class represents task statuses repository.
    /// </summary>
    public class TaskStatusesRepository : BaseRepository<TaskStatuses>, ITaskStatusesRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskStatusesRepository"/> class.
        /// </summary>
        /// <param name="dbContext">Application database context.</param>
        public TaskStatusesRepository(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <inheritdoc/>
        protected override DbSet<TaskStatuses> DbSet => this.DbContext.TaskStatuses;

        /// <inheritdoc/>
        public override async Task<int> UpdateAsync(int id, TaskStatuses newTaskStatus)
        {
            var taskStatusInDatabase = await this.DbSet.FindAsync(id);

            taskStatusInDatabase.Name = newTaskStatus.Name;
            taskStatusInDatabase.Description = newTaskStatus.Description;

            await this.SaveAsync();

            return taskStatusInDatabase.Id;
        }

        /// <inheritdoc/>
        public async Task<TaskStatuses> GetByNameAsync(string name)
        {
            return await this.DbSet.FindAsync(name);
        }
    }
}
