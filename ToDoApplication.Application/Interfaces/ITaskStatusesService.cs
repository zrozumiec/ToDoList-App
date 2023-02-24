using ToDoApplication.Application.DTOs;

namespace ToDoApplication.Application.Interfaces
{
    /// <summary>
    /// Interface for task statuses services.
    /// </summary>
    public interface ITaskStatusesService
    {
        /// <summary>
        /// Async method to add new status.
        /// </summary>
        /// <param name="taskStatusDto">Status Dto.</param>
        /// <returns>Added status id.</returns>
        /// <exception cref="ArgumentNullException">Throws when status to add is null.</exception>
        /// <exception cref="ArgumentException">Throws when status to add already exist in database.</exception>
        Task<int> AddAsync(TaskStatusesDto taskStatusDto);

        /// <summary>
        /// Async method to update existing status.
        /// </summary>
        /// <param name="id">Id of status to update.</param>
        /// <param name="taskStatusDto">New status Dto data.</param>
        /// <returns>Updated status id.</returns>
        /// <exception cref="ArgumentNullException">Throws when new status data is null.</exception>
        /// <exception cref="ArgumentException">Throws when status with given id doesn't exist.</exception>
        Task<int> UpdateAsync(int id, TaskStatusesDto taskStatusDto);

        /// <summary>
        /// Async method to delete existing status.
        /// </summary>
        /// <param name="id">Id of status to delete.</param>
        /// <returns>Deleted status id.</returns>
        /// <exception cref="ArgumentException">Throws when status with given id doesn't exist.</exception>
        Task<int> DeleteAsync(int id);

        /// <summary>
        /// Async method to return status with specified id.
        /// </summary>
        /// <param name="id">Status id.</param>
        /// <returns>Status with specified id.</returns>
        /// <exception cref="ArgumentException">Throws when status with given id doesn't exist.</exception>
        Task<TaskStatusesDto> GetByIdAsync(int id);

        /// <summary>
        /// Returns all statutes.
        /// </summary>
        /// <returns>All statutes.</returns>
        IEnumerable<TaskStatusesDto> GetAll();
    }
}
