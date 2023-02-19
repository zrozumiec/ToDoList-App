namespace ToDoApplication.Application.Interfaces.Base
{
    /// <summary>
    /// Base interface for services.
    /// </summary>
    /// <typeparam name="TDto">Dto item.</typeparam>
    public interface IBaseService<TDto>
    {
        /// <summary>
        /// Async method to add new item.
        /// </summary>
        /// <param name="itemDto">Item Dto.</param>
        /// <returns>Added item id.</returns>
        /// <exception cref="ArgumentNullException">Throws when item to add is null.</exception>
        Task<int> AddAsync(TDto itemDto);

        /// <summary>
        /// Async method to update existing item.
        /// </summary>
        /// <param name="id">Id of item to update.</param>
        /// <param name="itemDto">New item Dto data.</param>
        /// <returns>Updated item id.</returns>
        /// <exception cref="ArgumentNullException">Throws when new item data is null.</exception>
        /// <exception cref="ArgumentException">Throws when item with given id doesnt exist.</exception>
        Task<int> UpdateAsync(int id, TDto itemDto);

        /// <summary>
        /// Async method to delete existing item.
        /// </summary>
        /// <param name="id">Id of item to delete.</param>
        /// <returns>Deleted item id.</returns>
        /// <exception cref="ArgumentException">Throws when item with given id doesnt exist.</exception>
        Task<int> DeleteAsync(int id);

        /// <summary>
        /// Async method to return item with specified id.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns>Item with specified id.</returns>
        /// <exception cref="ArgumentException">Throws when item with given id doesnt exist.</exception>
        Task<TDto> GetByIdAsync(int id);

        /// <summary>
        /// Returns all items.
        /// </summary>
        /// <returns>All items.</returns>
        IEnumerable<TDto> GetAll();
    }
}
