using ToDoApplication.Domain.Interfaces.Base;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Domain.Interfaces
{
    /// <summary>
    /// Interface for task priority repository inherit from base interface repository.
    /// </summary>
    public interface ITaskPriorityRepository : IBaseRepository<TaskPriority>
    {
        /// <summary>
        /// Update existing task priority in database.
        /// </summary>
        /// <param name="id">Id of task priority to update.</param>
        /// <param name="newTaskPriority">New task priority data.</param>
        /// <returns>Updated task priority id.</returns>
        new Task<int> UpdateAsync(int id, TaskPriority newTaskPriority);

        /// <summary>
        /// Returns task priority with given name.
        /// </summary>
        /// <param name="name">Task priority name.</param>
        /// <returns>Task priority with specified name.</returns>
        Task<TaskPriority> GetByNameAsync(string name);
    }
}