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

        /// <inheritdoc/>
        public async Task<ToDoList> CheckIfExistInDataBaseWithSameNameAsync(string name)
        {
            return await this.DbSet.FirstOrDefaultAsync(x => x.Description.ToLower() == name.ToLower());
        }

        /// <inheritdoc/>
        public async Task CopyList(int listId)
        {
            var list = this.DbSet
                    .AsNoTracking()
                    .Include(x => x.User)
                    .Include(x => x.Tasks).ThenInclude(x => x.Category)
                    .Include(x => x.Tasks).ThenInclude(x => x.Status)
                    .Include(x => x.Tasks).ThenInclude(x => x.Priority)
                    .FirstOrDefault(x => x.Id == listId);

            if (list == null)
            {
                return;
            }

            list.Id = 0;
            list.Tile += "_Copy";
            list.Description += "_Copy";
            list.User = null;

            foreach (var task in list.Tasks)
            {
                task.Id = 0;
                task.Category = null;
                task.Priority = null;
                task.Status = null;
            }

            this.DbSet.Add(list);
            await this.SaveAsync();
        }

        /// <inheritdoc/>
        public override IEnumerable<ToDoList> GetAll()
        {
            return this.DbSet.Include(x => x.Tasks);
        }
    }
}
