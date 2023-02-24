using ToDoApplication.Domain.Interfaces.Base;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Domain.Interfaces
{
    /// <summary>
    /// Interface for task statuses repository inherit from base interface repository.
    /// </summary>
    public interface ITaskStatusesRepository : IBaseRepository<TaskStatuses>
    {
        /// <summary>
        /// Update existing task statuses in database.
        /// </summary>
        /// <param name="id">Id of task statuses to update.</param>
        /// <param name="newTaskStatus">New task statuses data.</param>
        /// <returns>Updated task statuses id.</returns>
        new Task<int> UpdateAsync(int id, TaskStatuses newTaskStatus);

        /// <summary>
        /// Returns task status with given name.
        /// </summary>
        /// <param name="name">Task status name.</param>
        /// <returns>Task status with specified name.</returns>
        Task<TaskStatuses> GetByNameAsync(string name);

        /// <summary>
        /// Checks by name if status already exist in database.
        /// </summary>
        /// <param name="name">Status name.</param>
        /// <returns>Return status specified by name.</returns>
        Task<TaskStatuses> CheckIfExistInDataBaseWithSameNameAsync(string name);
    }
}