using AutoMapper;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces;
using ToDoApplication.Application.Services.Base;
using ToDoApplication.Domain.Interfaces;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Application.Services
{
    /// <summary>
    /// Class contains service method for task priority.
    /// </summary>
    public class TaskPriorityService
        : BaseService<TaskPriorityDto, TaskPriority, ITaskPriorityRepository>, ITaskPriorityService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskPriorityService"/> class.
        /// </summary>
        /// <param name="mapper">Task priority mapper.</param>
        /// <param name="repository">Task priority repository.</param>
        public TaskPriorityService(IMapper mapper, ITaskPriorityRepository repository)
            : base(mapper, repository)
        {
        }

        /// <inheritdoc/>
        public override Task<int> AddAsync(TaskPriorityDto taskPriorityDto)
        {
            if (taskPriorityDto is null)
            {
                throw new ArgumentNullException(nameof(taskPriorityDto), "Task priority can not be null.");
            }

            var taskPriorityInDatabase = this.GetByNameInternalAsync(taskPriorityDto.Name);

            if (taskPriorityInDatabase is not null)
            {
                throw new ArgumentException("Task priority already exist in database.", nameof(taskPriorityDto));
            }

            return this.AddInternalAsync(taskPriorityDto);
        }

        private async Task<TaskCategoryDto> GetByNameInternalAsync(string taskCategoryName)
        {
            var taskCategoryInDatabase = await this.Repository.GetByNameAsync(taskCategoryName);

            return this.Mapper.Map<TaskCategoryDto>(taskCategoryInDatabase);
        }
    }
}