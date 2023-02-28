using ToDoApplication.Application.DTOs;

namespace ToDoApplication.Application.Interfaces
{
    /// <summary>
    /// Interface for task services.
    /// </summary>
    public interface IToDoTaskService
    {
        /// <summary>
        /// Async method to add new task.
        /// </summary>
        /// <param name="taskDto">Task Dto.</param>
        /// <returns>Added task id.</returns>
        /// <exception cref="ArgumentNullException">Throws when task to add is null.</exception>
        Task<int> AddAsync(ToDoTaskDto taskDto);

        /// <summary>
        /// Async method to update existing task.
        /// </summary>
        /// <param name="id">Id of task to update.</param>
        /// <param name="taskDto">New task Dto data.</param>
        /// <returns>Updated task id.</returns>
        /// <exception cref="ArgumentNullException">Throws when new task data is null.</exception>
        /// <exception cref="ArgumentException">Throws when task with given id doesn't exist.</exception>
        Task<int> UpdateAsync(int id, ToDoTaskDto taskDto);

        /// <summary>
        /// Async method to delete existing task.
        /// </summary>
        /// <param name="id">Id of task to delete.</param>
        /// <returns>Deleted task id.</returns>
        /// <exception cref="ArgumentException">Throws when task with given id doesn't exist.</exception>
        Task<int> DeleteAsync(int id);

        /// <summary>
        /// Async method to return task with specified id.
        /// </summary>
        /// <param name="id">Task id.</param>
        /// <returns>Task with specified id.</returns>
        /// <exception cref="ArgumentException">Throws when task with given id doesn't exist.</exception>
        Task<ToDoTaskDto> GetByIdAsync(int id);

        /// <summary>
        /// Gets all task for specified list.
        /// </summary>
        /// <param name="listId">ToDoList id.</param>
        /// <returns>All tasks included in specified list.</returns>
        IEnumerable<ToDoTaskDto> GetAll();

        /// <summary>
        /// Gets all task for specified list.
        /// </summary>
        /// <param name="listId">ToDoList id.</param>
        /// <returns>All tasks included in specified list.</returns>
        IEnumerable<ToDoTaskDto> GetAll(int listId);

        /// <summary>
        /// Fill list tasks.
        /// </summary>
        /// <param name="lists">Collection of list.</param>
        void FillListTasks(IEnumerable<ToDoListDto> lists);

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
        /// Gets task only for today.
        /// </summary>
        /// <param name="userLists">Users lists.</param>
        /// <returns>User task for today.</returns>
        IEnumerable<ToDoTaskDto> GetTaskForToday(IEnumerable<ToDoListDto> userLists);

        /// <summary>
        /// Turn on/off reminder for task.
        /// </summary>
        /// <param name="id">Task id.</param>
        /// <param name="turnOnOff">Bool value, false - turn off, true - turn on reminder.</param>
        /// <returns>Id of updated task.</returns>
        /// <exception cref="ArgumentException">Throws when task with given id does not exist in database.</exception>
        Task<int> TurnOnOffReminderAsync(int id, bool turnOnOff);

        /// <summary>
        /// Set reminder time for task.
        /// </summary>
        /// <param name="id">Task id.</param>
        /// <param name="reminderDate">Reminder date.</param>
        /// <returns>Id of task with reminder.</returns>
        /// <exception cref="ArgumentException">Throws when task does not exist in database or reminder date is low than create date.</exception>
        Task<int> SetReminderTimeAsync(int id, DateTimeOffset reminderDate);

        /// <summary>
        /// Check if reminder time occurs.
        /// </summary>
        /// <param name="id">Task id.</param>
        /// <returns>True - if reminder time occurs, false - otherwise.</returns>
        Task<bool> CheckIfReminderTimeOccursAsync(int id);
    }
}
