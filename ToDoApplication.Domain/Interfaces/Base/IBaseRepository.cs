using ToDoApplication.Domain.Models.Base;

namespace ToDoApplication.Domain.Interfaces.Base
{
    /// <summary>
    /// Interface for base repository.
    /// </summary>
    /// <typeparam name="T">ToDoApp item.</typeparam>
    public interface IBaseRepository<T>
        where T : BaseEntity
    {
        /// <summary>
        /// Add item of type T to database.
        /// </summary>
        /// <param name="item">Item of type T to add.</param>
        /// <returns>Added item id.</returns>
        Task<int> AddAsync(T item);

        /// <summary>
        /// Delete existing item from database.
        /// </summary>
        /// <param name="id">Item id to be deleted.</param>
        /// <returns>Deleted item id.</returns>
        Task<int> DeleteAsync(int id);

        /// <summary>
        /// Update existing item in database.
        /// </summary>
        /// <param name="id">Id of item to update.</param>
        /// <param name="newItem">New item data.</param>
        /// <returns>Updated item id.</returns>
        Task<int> UpdateAsync(int id, T newItem);

        /// <summary>
        /// Returns all items from database.
        /// </summary>
        /// <returns>All items of type T.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Returns item with given id.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns>Item with specified id.</returns>
        Task<T> GetByIdAsync(int id);
    }
}