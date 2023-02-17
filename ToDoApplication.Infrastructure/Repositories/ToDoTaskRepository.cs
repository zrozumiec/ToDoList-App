using Microsoft.EntityFrameworkCore;
using ToDoApplication.Domain.Interfaces;
using ToDoApplication.Domain.Models;
using ToDoApplication.Infrastructure.Repositories.Base;

namespace ToDoApplication.Infrastructure.Repositories
{
    /// <summary>
    /// Class represents ToDoTask repository.
    /// </summary>
    public class ToDoTaskRepository : BaseRepository<ToDoTask>, IToDoTaskRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoTaskRepository"/> class.
        /// </summary>
        /// <param name="dbContext">Application database context.</param>
        public ToDoTaskRepository(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <inheritdoc/>
        protected override DbSet<ToDoTask> DbSet => this.DbContext.ToDoTasks;

        /// <inheritdoc/>
        public override async Task<int> UpdateAsync(int id, ToDoTask newTask)
        {
            var toDoTaskInDatabase = await this.DbSet.FindAsync(id);

            toDoTaskInDatabase.Tile = newTask.Tile;
            toDoTaskInDatabase.Description = newTask.Description;
            toDoTaskInDatabase.DueDate = newTask.DueDate;
            toDoTaskInDatabase.Reminder = newTask.Reminder;
            toDoTaskInDatabase.ReminderDate = newTask.ReminderDate;
            toDoTaskInDatabase.Daily = newTask.Daily;
            toDoTaskInDatabase.Important = newTask.Important;
            toDoTaskInDatabase.IsCompleted = newTask.IsCompleted;

            await this.SaveAsync();

            return toDoTaskInDatabase.Id;
        }
    }
}
