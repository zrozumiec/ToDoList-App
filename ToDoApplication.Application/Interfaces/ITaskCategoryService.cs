using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces.Base;

namespace ToDoApplication.Application.Interfaces
{
    /// <summary>
    /// Interface for task category services.
    /// </summary>
    public interface ITaskCategoryService : IBaseService<TaskCategoryDto>
    {
        new Task<int> AddAsync(TaskCategoryDto taskCategoryDto);
    }
}
