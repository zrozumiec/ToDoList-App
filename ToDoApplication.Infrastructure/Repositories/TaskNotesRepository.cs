using Microsoft.EntityFrameworkCore;
using ToDoApplication.Domain.Interfaces;
using ToDoApplication.Domain.Models;
using ToDoApplication.Infrastructure.Repositories.Base;

namespace ToDoApplication.Infrastructure.Repositories
{
    /// <summary>
    /// Class represents task notes repository.
    /// </summary>
    public class TaskNotesRepository : BaseRepository<TaskNotes>, ITaskNotesRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskNotesRepository"/> class.
        /// </summary>
        /// <param name="dbContext">Application database context.</param>
        public TaskNotesRepository(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <inheritdoc/>
        protected override DbSet<TaskNotes> DbSet => this.DbContext.TaskNotes;

        /// <inheritdoc/>
        public override async Task<int> UpdateAsync(int id, TaskNotes newTaskNotes)
        {
            var taskNotesInDatabase = await this.DbSet.FindAsync(id);

            taskNotesInDatabase.Description = newTaskNotes.Description;

            await this.SaveAsync();

            return taskNotesInDatabase.Id;
        }
    }
}
