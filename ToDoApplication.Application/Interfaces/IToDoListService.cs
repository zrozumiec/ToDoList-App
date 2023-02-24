using ToDoApplication.Application.DTOs;

namespace ToDoApplication.Application.Interfaces
{
    /// <summary>
    /// Interface for ToDoList services.
    /// </summary>
    public interface IToDoListService
    {
        /// <summary>
        /// Async method to add new ToDoList.
        /// </summary>
        /// <param name="toDoListDto">ToDoList Dto.</param>
        /// <returns>Added ToDoList id.</returns>
        /// <exception cref="ArgumentNullException">Throws when ToDoList to add is null.</exception>
        Task<int> AddAsync(ToDoListDto toDoListDto);

        /// <summary>
        /// Async method to update existing ToDoList.
        /// </summary>
        /// <param name="id">Id of ToDoList to update.</param>
        /// <param name="toDoListDto">New ToDoList Dto data.</param>
        /// <returns>Updated ToDoList id.</returns>
        /// <exception cref="ArgumentNullException">Throws when new ToDoList data is null.</exception>
        /// <exception cref="ArgumentException">Throws when ToDoList with given id doesn't exist.</exception>
        Task<int> UpdateAsync(int id, ToDoListDto toDoListDto);

        /// <summary>
        /// Async method to delete existing ToDoList.
        /// </summary>
        /// <param name="id">Id of ToDoList to delete.</param>
        /// <returns>Deleted ToDoList id.</returns>
        /// <exception cref="ArgumentException">Throws when ToDoList with given id doesn't exist.</exception>
        Task<int> DeleteAsync(int id);

        /// <summary>
        /// Async method to return ToDoList with specified id.
        /// </summary>
        /// <param name="id">ToDoList id.</param>
        /// <returns>ToDoList with specified id.</returns>
        /// <exception cref="ArgumentException">Throws when ToDoList with given id doesn't exist.</exception>
        Task<ToDoListDto> GetByIdAsync(int id);

        /// <summary>
        /// Returns all ToDoList.
        /// </summary>
        /// <returns>All ToDoList.</returns>
        IEnumerable<ToDoListDto> GetAll();

        Task<int> ChangeVisibility(int id, bool visibility);
    }
}
