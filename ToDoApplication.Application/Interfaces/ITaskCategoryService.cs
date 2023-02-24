using ToDoApplication.Application.DTOs;

namespace ToDoApplication.Application.Interfaces
{
    /// <summary>
    /// Interface for task category services.
    /// </summary>
    public interface ITaskCategoryService
    {
        /// <summary>
        /// Async method to add new category.
        /// </summary>
        /// <param name="taskCategoryDto">Category Dto.</param>
        /// <returns>Added category id.</returns>
        /// <exception cref="ArgumentNullException">Throws when category to add is null.</exception>
        /// <exception cref="ArgumentException">Throws when category to add already exist in database.</exception>
        Task<int> AddAsync(TaskCategoryDto taskCategoryDto);

        /// <summary>
        /// Async method to update existing category.
        /// </summary>
        /// <param name="id">Id of category to update.</param>
        /// <param name="taskCategoryDto">New category Dto data.</param>
        /// <returns>Updated category id.</returns>
        /// <exception cref="ArgumentNullException">Throws when new category data is null.</exception>
        /// <exception cref="ArgumentException">Throws when category with given id doesn't exist or category already exist.</exception>
        Task<int> UpdateAsync(int id, TaskCategoryDto taskCategoryDto);

        /// <summary>
        /// Async method to delete existing category.
        /// </summary>
        /// <param name="id">Id of category to delete.</param>
        /// <returns>Deleted category id.</returns>
        /// <exception cref="ArgumentException">Throws when category with given id doesn't exist.</exception>
        Task<int> DeleteAsync(int id);

        /// <summary>
        /// Async method to return category with specified id.
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <returns>Category with specified id.</returns>
        /// <exception cref="ArgumentException">Throws when category with given id doesn't exist.</exception>
        Task<TaskCategoryDto> GetByIdAsync(int id);

        /// <summary>
        /// Returns all categories.
        /// </summary>
        /// <returns>All categories.</returns>
        IEnumerable<TaskCategoryDto> GetAll();
    }
}
