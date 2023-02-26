using Microsoft.EntityFrameworkCore;
using ToDoApplication.Domain.Interfaces.Base;
using ToDoApplication.Domain.Models.Base;

namespace ToDoApplication.Infrastructure.Repositories.Base
{
    /// <summary>
    /// Abstract base repository class.
    /// </summary>
    /// <typeparam name="T">ToDoApp item.</typeparam>
    public abstract class BaseRepository<T> : IBaseRepository<T>
        where T : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class.
        /// </summary>
        /// <param name="dbContext">Application database context.</param>
        protected BaseRepository(AppDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        /// <summary>
        /// Gets database context.
        /// </summary>
        protected AppDbContext DbContext { get; }

        /// <summary>
        /// Gets DbSet of type T.
        /// </summary>
        protected abstract DbSet<T> DbSet { get; }

        /// <inheritdoc/>
        public virtual async Task<int> AddAsync(T item)
        {
            this.DbSet.Add(item);
            await this.SaveAsync();

            return item.Id;
        }

        /// <inheritdoc/>
        public virtual async Task<int> DeleteAsync(int id)
        {
            var item = await this.DbSet.FindAsync(id);

            this.DbSet.Remove(item);
            await this.SaveAsync();

            return item.Id;
        }

        /// <inheritdoc/>
        public abstract Task<int> UpdateAsync(int id, T newItem);

        /// <inheritdoc/>
        public virtual IEnumerable<T> GetAll()
        {
            return this.DbSet;
        }

        /// <inheritdoc/>s
        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await this.DbSet.FindAsync(id);
        }

        /// <summary>
        /// Save all changes made in db context.
        /// </summary>
        /// <returns>Result of save operation.</returns>
        protected async Task<int> SaveAsync()
        {
            return await this.DbContext.SaveChangesAsync();
        }
    }
}
