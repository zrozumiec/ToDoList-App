using ToDoApplication.Application.DTOs;

namespace ToDoApplication.Application.Interfaces
{
    /// <summary>
    /// Interface for task priority services.
    /// </summary>
    public interface ITaskPriorityService
    {
        /// <summary>
        /// Async method to add new priority.
        /// </summary>
        /// <param name="priorityDto">Priority Dto.</param>
        /// <returns>Added priority id.</returns>
        /// <exception cref="ArgumentNullException">Throws when priority to add is null.</exception>
        /// <exception cref="ArgumentException">Throws when priority to add already exist in database.</exception>
        Task<int> AddAsync(TaskPriorityDto priorityDto);

        /// <summary>
        /// Async method to update existing priority.
        /// </summary>
        /// <param name="id">Id of priority to update.</param>
        /// <param name="priorityDto">New priority Dto data.</param>
        /// <returns>Updated priority id.</returns>
        /// <exception cref="ArgumentNullException">Throws when new priority data is null.</exception>
        /// <exception cref="ArgumentException">Throws when priority with given id doesn't exist.</exception>
        Task<int> UpdateAsync(int id, TaskPriorityDto priorityDto);

        /// <summary>
        /// Async method to delete existing priority.
        /// </summary>
        /// <param name="id">Id of priority to delete.</param>
        /// <returns>Deleted priority id.</returns>
        /// <exception cref="ArgumentException">Throws when priority with given id doesn't exist.</exception>
        Task<int> DeleteAsync(int id);

        /// <summary>
        /// Async method to return priority with specified id.
        /// </summary>
        /// <param name="id">Priority id.</param>
        /// <returns>Priority with specified id.</returns>
        /// <exception cref="ArgumentException">Throws when priority with given id doesn't exist.</exception>
        Task<TaskPriorityDto> GetByIdAsync(int id);

        /// <summary>
        /// Returns all priorities.
        /// </summary>
        /// <returns>All priorities.</returns>
        IEnumerable<TaskPriorityDto> GetAll();
    }
}
