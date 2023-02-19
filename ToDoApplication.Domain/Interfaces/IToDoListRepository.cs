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
    }
}