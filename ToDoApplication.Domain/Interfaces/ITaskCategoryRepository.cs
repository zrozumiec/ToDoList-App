using ToDoApplication.Domain.Interfaces.Base;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Domain.Interfaces
{
    /// <summary>
    /// Interface for task category repository inherit from base interface repository.
    /// </summary>
    public interface ITaskCategoryRepository : IBaseRepository<TaskCategory>
    {
        /// <summary>
        /// Update existing task category in database.
        /// </summary>
        /// <param name="id">Id of task category to update.</param>
        /// <param name="newTaskCategory">New task category data.</param>
        /// <returns>Updated task category id.</returns>
        new Task<int> UpdateAsync(int id, TaskCategory newTaskCategory);

        /// <summary>
        /// Returns task category with given name.
        /// </summary>
        /// <param name="name">Task category name.</param>
        /// <returns>Task category with specified name.</returns>
        Task<TaskCategory> GetByNameAsync(string name);

        /// <summary>
        /// Checks by name if category already exist in database.
        /// </summary>
        /// <param name="name">Category name.</param>
        /// <returns>Return category specified by name.</returns>
        Task<TaskCategory> CheckIfExistInDataBaseWithSameNameAsync(string name);
    }
}