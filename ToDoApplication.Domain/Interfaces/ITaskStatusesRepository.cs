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
    }
}