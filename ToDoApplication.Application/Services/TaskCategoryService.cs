using AutoMapper;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces;
using ToDoApplication.Domain.Interfaces;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Application.Services
{
    /// <summary>
    /// Class contains service method for task category.
    /// </summary>
    public class TaskCategoryService : ITaskCategoryService
    {
        private readonly IMapper mapper;
        private readonly ITaskCategoryRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskCategoryService"/> class.
        /// </summary>
        /// <param name="mapper">Task category mapper.</param>
        /// <param name="repository">Task category repository.</param>
        public TaskCategoryService(IMapper mapper, ITaskCategoryRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        /// <inheritdoc/>
        public Task<int> AddAsync(TaskCategoryDto taskCategoryDto)
        {
            if (taskCategoryDto is null)
            {
                throw new ArgumentNullException(nameof(taskCategoryDto), "Task category can not be null.");
            }

            var taskCategoryInDatabase = this.GetByNameInternalAsync(taskCategoryDto.Name).Result;

            if (taskCategoryInDatabase is not null)
            {
                throw new ArgumentException("Task category already exist in database.", nameof(taskCategoryDto));
            }

            return this.AddInternalAsync(taskCategoryDto);
        }

        /// <inheritdoc/>
        public Task<int> DeleteAsync(int id)
        {
            var itemInDatabaseDto = this.GetByIdInternalAsync(id).Result;

            if (itemInDatabaseDto is null)
            {
                throw new ArgumentException("Category with given id does not exist in database.", nameof(id));
            }

            return this.DeleteInternalAsync(id);
        }

        /// <inheritdoc/>
        public Task<int> UpdateAsync(int id, TaskCategoryDto taskCategoryDto)
        {
            if (taskCategoryDto is null)
            {
                throw new ArgumentNullException(nameof(taskCategoryDto), "Category can not be null.");
            }

            var itemInDatabase = this.GetByIdInternalAsync(id).Result;

            if (itemInDatabase is null)
            {
                throw new ArgumentException("Category with given id does not exist in database.", nameof(id));
            }

            var itemInDatabaseWithSameName = this.repository.CheckIfExistInDataBaseWithSameNameAsync(taskCategoryDto.Name).Result;

            if (itemInDatabaseWithSameName is not null)
            {
                throw new ArgumentException("Category with given name already exist in database.", nameof(id));
            }

            return this.UpdateInternalAsync(id, taskCategoryDto);
        }

        /// <inheritdoc/>
        public Task<TaskCategoryDto> GetByIdAsync(int id)
        {
            var itemDto = this.GetByIdInternalAsync(id);

            if (itemDto.Result is null)
            {
                throw new ArgumentException("Category with given id does not exist in database.", nameof(id));
            }

            return itemDto;
        }

        /// <inheritdoc/>
        public IEnumerable<TaskCategoryDto> GetAll()
        {
            var items = this.repository.GetAll();

            return this.mapper.Map<IEnumerable<TaskCategoryDto>>(items);
        }

        /// <summary>
        /// Async method to add item.
        /// </summary>
        /// <param name="itemDto">Item to add.</param>
        /// <returns>Added item id.</returns>
        private async Task<int> AddInternalAsync(TaskCategoryDto itemDto)
        {
            var item = this.mapper.Map<TaskCategory>(itemDto);

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
        private async Task<TaskCategoryDto> GetByIdInternalAsync(int id)
        {
            var itemInDatabase = await this.repository.GetByIdAsync(id);

            return this.mapper.Map<TaskCategoryDto>(itemInDatabase);
        }

        /// <summary>
        /// Async method to update item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="itemDto">New item data.</param>
        /// <returns>Updated item id.</returns>
        private async Task<int> UpdateInternalAsync(int id, TaskCategoryDto itemDto)
        {
            var item = this.mapper.Map<TaskCategory>(itemDto);

            return await this.repository.UpdateAsync(id, item);
        }

        private async Task<TaskCategoryDto> GetByNameInternalAsync(string taskCategoryName)
        {
            var taskCategoryInDatabase = await this.repository.GetByNameAsync(taskCategoryName);

            return this.mapper.Map<TaskCategoryDto>(taskCategoryInDatabase);
        }
    }
}
