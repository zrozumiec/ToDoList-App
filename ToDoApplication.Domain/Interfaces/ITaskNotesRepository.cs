using ToDoApplication.Domain.Interfaces.Base;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Domain.Interfaces
{
    /// <summary>
    /// Interface for task notes repository inherit from base interface repository.
    /// </summary>
    public interface ITaskNotesRepository : IBaseRepository<TaskNotes>
    {
        /// <summary>
        /// Update existing task notes in database.
        /// </summary>
        /// <param name="id">Id of task notes to update.</param>
        /// <param name="newTaskNotes">New task notes data.</param>
        /// <returns>Updated task notes id.</returns>
        new Task<int> UpdateAsync(int id, TaskNotes newTaskNotes);
    }
}