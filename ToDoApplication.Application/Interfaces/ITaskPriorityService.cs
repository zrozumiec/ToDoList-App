using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces.Base;

namespace ToDoApplication.Application.Interfaces
{
    /// <summary>
    /// Interface for task priority services.
    /// </summary>
    public interface ITaskPriorityService : IBaseService<TaskPriorityDto>
    {
        new Task<int> AddAsync(TaskPriorityDto taskPriorityDto);
    }
}
