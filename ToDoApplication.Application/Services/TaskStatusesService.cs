using AutoMapper;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces;
using ToDoApplication.Application.Services.Base;
using ToDoApplication.Domain.Interfaces;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Application.Services
{
    /// <summary>
    /// Class contains service method for task statuses.
    /// </summary>
    public class TaskStatusesService
        : BaseService<TaskStatusesDto, TaskStatuses, ITaskStatusesRepository>, ITaskStatusesService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskStatusesService"/> class.
        /// </summary>
        /// <param name="mapper">Task statuses mapper.</param>
        /// <param name="repository">Task statuses repository.</param>
        public TaskStatusesService(IMapper mapper, ITaskStatusesRepository repository)
            : base(mapper, repository)
        {
        }

        /// <inheritdoc/>
        public override Task<int> AddAsync(TaskStatusesDto taskStatusesDto)
        {
            if (taskStatusesDto is null)
            {
                throw new ArgumentNullException(nameof(taskStatusesDto), "Task statuses can not be null.");
            }

            var taskStatusesInDatabase = this.GetByNameInternalAsync(taskStatusesDto.Name);

            if (taskStatusesInDatabase is not null)
            {
                throw new ArgumentException("Task priority already exist in database.", nameof(taskStatusesDto));
            }

            return this.AddInternalAsync(taskStatusesDto);
        }

        private async Task<TaskCategoryDto> GetByNameInternalAsync(string taskCategoryName)
        {
            var taskCategoryInDatabase = await this.Repository.GetByNameAsync(taskCategoryName);

            return this.Mapper.Map<TaskCategoryDto>(taskCategoryInDatabase);
        }
    }
}