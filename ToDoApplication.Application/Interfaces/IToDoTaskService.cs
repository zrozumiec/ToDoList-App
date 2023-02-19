using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces.Base;

namespace ToDoApplication.Application.Interfaces
{
    /// <summary>
    /// Interface for task services.
    /// </summary>
    public interface IToDoTaskService : IBaseService<ToDoTaskDto>
    {
        /// <summary>
        /// Returns all daily tasks.
        /// </summary>
        /// <returns>Daily tasks.</returns>
        IEnumerable<ToDoTaskDto> GetDailyTasks();

        /// <summary>
        /// Returns all important tasks.
        /// </summary>
        /// <returns>Important tasks.</returns>
        IEnumerable<ToDoTaskDto> GetImportantTasks();

        /// <summary>
        /// Returns all completed tasks.
        /// </summary>
        /// <returns>All completed tasks.</returns>
        IEnumerable<ToDoTaskDto> GetCompletedTasks();

        /// <summary>
        /// Returns all uncompleted tasks.
        /// </summary>
        /// <returns>All uncompleted tasks.</returns>
        IEnumerable<ToDoTaskDto> GetUncompletedTasks();

        /// <summary>
        /// Returns all tasks which should be done today.
        /// </summary>
        /// <returns>Tasks which should be done today.</returns>
        IEnumerable<ToDoTaskDto> GetTaskForToday();

        /// <summary>
        /// Turn on/off reminder for task.
        /// </summary>
        /// <param name="id">Task id.</param>
        /// <param name="turnOnOff">Bool value, false - turn off, true - turn on reminder.</param>
        /// <returns>Id of updated task.</returns>
        /// <exception cref="ArgumentException">Throws when task with given id does not exist in database.</exception>
        Task<int> TurnOnOffReminder(int id, bool turnOnOff);
    }
}
