using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces.Base;

namespace ToDoApplication.Application.Interfaces
{
    /// <summary>
    /// Interface for ToDoList services.
    /// </summary>
    public interface IToDoListService : IBaseService<ToDoListDto>
    {
        new Task<int> AddAsync(ToDoListDto toDoListDto);

        Task<int> ChangeVisibility(int id, bool visibility);
    }
}
