using Microsoft.EntityFrameworkCore;
using ToDoApplication.Domain.Interfaces;
using ToDoApplication.Domain.Models;
using ToDoApplication.Infrastructure.Repositories.Base;

namespace ToDoApplication.Infrastructure.Repositories
{
    /// <summary>
    /// Class represents task priority repository.
    /// </summary>
    public class TaskPriorityRepository : BaseRepository<TaskPriority>, ITaskPriorityRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskPriorityRepository"/> class.
        /// </summary>
        /// <param name="dbContext">Application database context.</param>
        public TaskPriorityRepository(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <inheritdoc/>
        protected override DbSet<TaskPriority> DbSet => this.DbContext.TaskPriorities;

        /// <inheritdoc/>
        public override async Task<int> UpdateAsync(int id, TaskPriority newTaskPriority)
        {
            var taskPriorityInDatabase = await this.DbSet.FindAsync(id);

            taskPriorityInDatabase.Name = newTaskPriority.Name;

            await this.SaveAsync();

            return taskPriorityInDatabase.Id;
        }

        /// <inheritdoc/>
        public async Task<TaskPriority> GetByNameAsync(string name)
        {
            return await this.DbSet.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }

        /// <inheritdoc/>
        public async Task<TaskPriority> CheckIfExistInDataBaseWithSameNameAsync(string name)
        {
            return await this.DbSet.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }
    }
}
