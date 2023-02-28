using AutoMapper;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces;
using ToDoApplication.Domain.Interfaces;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Application.Services
{
    /// <summary>
    /// Class contains service method for task statuses.
    /// </summary>
    public class TaskStatusesService : ITaskStatusesService
    {
        private readonly IMapper mapper;
        private readonly ITaskStatusesRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskStatusesService"/> class.
        /// </summary>
        /// <param name="mapper">Task statuses mapper.</param>
        /// <param name="repository">Task statuses repository.</param>
        public TaskStatusesService(IMapper mapper, ITaskStatusesRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        /// <inheritdoc/>
        public Task<int> AddAsync(TaskStatusesDto taskStatusDto)
        {
            if (taskStatusDto is null)
            {
                throw new ArgumentNullException(nameof(taskStatusDto), "Task statuses can not be null.");
            }

            var taskStatusesInDatabase = this.GetByNameInternalAsync(taskStatusDto.Name).Result;

            if (taskStatusesInDatabase is not null)
            {
                throw new ArgumentException("Task statuses already exist in database.", nameof(taskStatusDto));
            }

            return this.AddInternalAsync(taskStatusDto);
        }

        /// <inheritdoc/>
        public Task<int> DeleteAsync(int id)
        {
            var statusInDatabaseDto = this.GetByIdInternalAsync(id).Result;

            if (statusInDatabaseDto is null)
            {
                throw new ArgumentException("Status with given id does not exist in database.", nameof(id));
            }

            return this.DeleteInternalAsync(id);
        }

        /// <inheritdoc/>
        public Task<int> UpdateAsync(int id, TaskStatusesDto taskStatusDto)
        {
            if (taskStatusDto is null)
            {
                throw new ArgumentNullException(nameof(taskStatusDto), "Status can not be null.");
            }

            var statusInDatabase = this.GetByIdInternalAsync(id).Result;

            if (statusInDatabase is null)
            {
                throw new ArgumentException("Status with given id does not exist in database.", nameof(id));
            }

            var itemInDatabaseWithSameName = this.repository.CheckIfExistInDataBaseWithSameNameAsync(taskStatusDto.Name).Result;

            if (itemInDatabaseWithSameName is not null && itemInDatabaseWithSameName.Name != statusInDatabase.Name)
            {
                throw new ArgumentException("Status with given name already exist in database.", nameof(id));
            }

            return this.UpdateInternalAsync(id, taskStatusDto);
        }

        /// <inheritdoc/>
        public Task<TaskStatusesDto> GetByIdAsync(int id)
        {
            var statusDto = this.GetByIdInternalAsync(id);

            if (statusDto.Result is null)
            {
                throw new ArgumentException("Status with given id does not exist in database.", nameof(id));
            }

            return statusDto;
        }

        /// <inheritdoc/>
        public IEnumerable<TaskStatusesDto> GetAll()
        {
            var statuses = this.repository.GetAll();

            return this.mapper.Map<IEnumerable<TaskStatusesDto>>(statuses);
        }

        /// <summary>
        /// Async method to add item.
        /// </summary>
        /// <param name="statusDto">Item to add.</param>
        /// <returns>Added item id.</returns>
        private async Task<int> AddInternalAsync(TaskStatusesDto statusDto)
        {
            var status = this.mapper.Map<TaskStatuses>(statusDto);

            return await this.repository.AddAsync(status);
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
        private async Task<TaskStatusesDto> GetByIdInternalAsync(int id)
        {
            var statusInDatabase = await this.repository.GetByIdAsync(id);

            return this.mapper.Map<TaskStatusesDto>(statusInDatabase);
        }

        /// <summary>
        /// Async method to update item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="statusDto">New item data.</param>
        /// <returns>Updated item id.</returns>
        private async Task<int> UpdateInternalAsync(int id, TaskStatusesDto statusDto)
        {
            var status = this.mapper.Map<TaskStatuses>(statusDto);

            return await this.repository.UpdateAsync(id, status);
        }

        private async Task<TaskStatusesDto> GetByNameInternalAsync(string taskStatusName)
        {
            var taskStatusInDatabase = await this.repository.GetByNameAsync(taskStatusName);

            return this.mapper.Map<TaskStatusesDto>(taskStatusInDatabase);
        }
    }
}