using Microsoft.EntityFrameworkCore;
using ToDoApplication.Domain.Interfaces;
using ToDoApplication.Domain.Models;
using ToDoApplication.Infrastructure.Repositories.Base;

namespace ToDoApplication.Infrastructure.Repositories
{
    /// <summary>
    /// Class represents ToDoList repository.
    /// </summary>
    public class ToDoListRepository : BaseRepository<ToDoList>, IToDoListRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoListRepository"/> class.
        /// </summary>
        /// <param name="dbContext">Application database context.</param>
        public ToDoListRepository(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <inheritdoc/>
        protected override DbSet<ToDoList> DbSet => this.DbContext.ToDoLists;

        /// <inheritdoc/>
        public override async Task<int> UpdateAsync(int id, ToDoList newToDoList)
        {
            var toDoListInDatabase = await this.DbSet.FindAsync(id);

            toDoListInDatabase.Tile = newToDoList.Tile;
            toDoListInDatabase.Description = newToDoList.Description;
            toDoListInDatabase.IsHidden = newToDoList.IsHidden;

            await this.SaveAsync();

            return toDoListInDatabase.Id;
        }

        /// <inheritdoc/>
        public async Task<ToDoList> GetByNameAsync(string name)
        {
            return await this.DbSet.FirstOrDefaultAsync(x => x.Tile.ToLower() == name.ToLower());
        }
    }
}
