using AutoMapper;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces;
using ToDoApplication.Application.Services.Base;
using ToDoApplication.Domain.Interfaces;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Application.Services
{
    /// <summary>
    /// Class contains service method for task category.
    /// </summary>
    public class TaskCategoryService
        : BaseService<TaskCategoryDto, TaskCategory, ITaskCategoryRepository>, ITaskCategoryService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskCategoryService"/> class.
        /// </summary>
        /// <param name="mapper">Task category mapper.</param>
        /// <param name="repository">Task category repository.</param>
        public TaskCategoryService(IMapper mapper, ITaskCategoryRepository repository)
            : base(mapper, repository)
        {
        }

        /// <inheritdoc/>
        public override Task<int> AddAsync(TaskCategoryDto taskCategoryDto)
        {
            if (taskCategoryDto is null)
            {
                throw new ArgumentNullException(nameof(taskCategoryDto), "Task category can not be null.");
            }

            var taskCategoryInDatabase = this.GetByNameInternalAsync(taskCategoryDto.Name);

            if (taskCategoryInDatabase is not null)
            {
                throw new ArgumentException("Task category already exist in database.", nameof(taskCategoryDto));
            }

            return this.AddInternalAsync(taskCategoryDto);
        }

        private async Task<TaskCategoryDto> GetByNameInternalAsync(string taskCategoryName)
        {
            var taskCategoryInDatabase = await this.Repository.GetByNameAsync(taskCategoryName);

            return this.Mapper.Map<TaskCategoryDto>(taskCategoryInDatabase);
        }
    }
}
