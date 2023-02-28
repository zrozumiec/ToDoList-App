using ToDoApplication.Domain.Interfaces.Base;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Domain.Interfaces
{
    /// <summary>
    /// Interface for ToDoList repository inherit from base interface repository.
    /// </summary>
    public interface IToDoListRepository : IBaseRepository<ToDoList>
    {
        /// <summary>
        /// Update existing ToDoList in database.
        /// </summary>
        /// <param name="id">Id of ToDoList to update.</param>
        /// <param name="newToDoList">New ToDoList data.</param>
        /// <returns>Updated ToDoList id.</returns>
        new Task<int> UpdateAsync(int id, ToDoList newToDoList);

        /// <summary>
        /// Returns list with given name.
        /// </summary>
        /// <param name="name">List name.</param>
        /// <returns>List with specified name.</returns>
        Task<ToDoList> GetByNameAsync(string name);

        /// <summary>
        /// Checks by name if list already exist in database.
        /// </summary>
        /// <param name="name">List name.</param>
        /// <returns>Return list specified by name.</returns>
        Task<ToDoList> CheckIfExistInDataBaseWithSameNameAsync(string name);

        /// <summary>
        /// Copy existing list.
        /// </summary>
        /// <param name="listId">List id.</param>
        /// <returns>Copy of existing list.</returns>
        Task CopyList(int listId);
    }
}