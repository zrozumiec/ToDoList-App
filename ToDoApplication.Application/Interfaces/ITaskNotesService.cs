using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces.Base;

namespace ToDoApplication.Application.Interfaces
{
    /// <summary>
    /// Interface for task notes services.
    /// </summary>
    public interface ITaskNotesService : IBaseService<TaskNotesDto>
    {
    }
}
