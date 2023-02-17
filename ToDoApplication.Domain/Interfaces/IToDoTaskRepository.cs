using ToDoApplication.Domain.Interfaces.Base;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Domain.Interfaces
{
    /// <summary>
    /// Interface for task repository inherit from base interface repository.
    /// </summary>
    public interface IToDoTaskRepository : IBaseRepository<ToDoTask>
    {
        /// <summary>
        /// Update existing ToDoTask in database.
        /// </summary>
        /// <param name="id">Id of ToDoTask to update.</param>
        /// <param name="newTask">New ToDoToDoTaskList data.</param>
        /// <returns>Updated ToDoTask id.</returns>
        new Task<int> UpdateAsync(int id, ToDoTask newTask);
    }
}