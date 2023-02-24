using AutoMapper;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces;
using ToDoApplication.Domain.Interfaces;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Application.Services
{
    /// <summary>
    /// Class contains service method for task priority.
    /// </summary>
    public class TaskPriorityService : ITaskPriorityService
    {
        private readonly IMapper mapper;
        private readonly ITaskPriorityRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskPriorityService"/> class.
        /// </summary>
        /// <param name="mapper">Task priority mapper.</param>
        /// <param name="repository">Task priority repository.</param>
        public TaskPriorityService(IMapper mapper, ITaskPriorityRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        /// <inheritdoc/>
        public Task<int> AddAsync(TaskPriorityDto taskPriorityDto)
        {
            if (taskPriorityDto is null)
            {
                throw new ArgumentNullException(nameof(taskPriorityDto), "Task priority can not be null.");
            }

            var taskPriorityInDatabase = this.GetByNameInternalAsync(taskPriorityDto.Name).Result;

            if (taskPriorityInDatabase is not null)
            {
                throw new ArgumentException("Task priority already exist in database.", nameof(taskPriorityDto));
            }

            return this.AddInternalAsync(taskPriorityDto);
        }

        /// <inheritdoc/>
        public Task<int> DeleteAsync(int id)
        {
            var itemInDatabaseDto = this.GetByIdInternalAsync(id).Result;

            if (itemInDatabaseDto is null)
            {
                throw new ArgumentException("Priority with given id does not exist in database.", nameof(id));
            }

            return this.DeleteInternalAsync(id);
        }

        /// <inheritdoc/>
        public Task<int> UpdateAsync(int id, TaskPriorityDto taskPriorityDto)
        {
            if (taskPriorityDto is null)
            {
                throw new ArgumentNullException(nameof(taskPriorityDto), "Priority can not be null.");
            }

            var itemInDatabase = this.GetByIdInternalAsync(id).Result;

            if (itemInDatabase is null)
            {
                throw new ArgumentException("Priority with given id does not exist in database.", nameof(id));
            }

            var itemInDatabaseWithSameName = this.repository.CheckIfExistInDataBaseWithSameNameAsync(taskPriorityDto.Name).Result;

            if (itemInDatabaseWithSameName is not null)
            {
                throw new ArgumentException("Priority with given name already exist in database.", nameof(id));
            }

            return this.UpdateInternalAsync(id, taskPriorityDto);
        }

        /// <inheritdoc/>
        public Task<TaskPriorityDto> GetByIdAsync(int id)
        {
            var itemDto = this.GetByIdInternalAsync(id);

            if (itemDto.Result is null)
            {
                throw new ArgumentException("Priority with given id does not exist in database.", nameof(id));
            }

            return itemDto;
        }

        /// <inheritdoc/>
        public IEnumerable<TaskPriorityDto> GetAll()
        {
            var items = this.repository.GetAll();

            return this.mapper.Map<IEnumerable<TaskPriorityDto>>(items);
        }

        /// <summary>
        /// Async method to add item.
        /// </summary>
        /// <param name="itemDto">Item to add.</param>
        /// <returns>Added item id.</returns>
        private async Task<int> AddInternalAsync(TaskPriorityDto itemDto)
        {
            var item = this.mapper.Map<TaskPriority>(itemDto);

            return await this.repository.AddAsync(item);
        }

        /// <summary>
        /// Async method to delete item.
        /// </summary>
        /// <param name="id">Item id to be deleted.</param>
        /// <returns>Deleted item id.</returns>
        private async Task<int> DeleteInternalAsync(int id)
        {
            return await this.repository.DeleteAsync(id);
        }

        /// <summary>
        /// Async method to return item with specified id.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns>Item with specified id.</returns>
        private async Task<TaskPriorityDto> GetByIdInternalAsync(int id)
        {
            var itemInDatabase = await this.repository.GetByIdAsync(id);

            return this.mapper.Map<TaskPriorityDto>(itemInDatabase);
        }

        /// <summary>
        /// Async method to update item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="itemDto">New item data.</param>
        /// <returns>Updated item id.</returns>
        private async Task<int> UpdateInternalAsync(int id, TaskPriorityDto itemDto)
        {
            var item = this.mapper.Map<TaskPriority>(itemDto);

            return await this.repository.UpdateAsync(id, item);
        }

        private async Task<TaskPriorityDto> GetByNameInternalAsync(string taskPriorityName)
        {
            var taskPriorityInDatabase = await this.repository.GetByNameAsync(taskPriorityName);

            return this.mapper.Map<TaskPriorityDto>(taskPriorityInDatabase);
        }
    }
}