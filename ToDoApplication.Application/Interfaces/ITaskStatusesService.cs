using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces.Base;

namespace ToDoApplication.Application.Interfaces
{
    /// <summary>
    /// Interface for task statuses services.
    /// </summary>
    public interface ITaskStatusesService : IBaseService<TaskStatusesDto>
    {
        new Task<int> AddAsync(TaskStatusesDto taskStatusesDto);
    }
}
