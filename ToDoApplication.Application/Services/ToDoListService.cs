using AutoMapper;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces;
using ToDoApplication.Domain.Interfaces;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Application.Services
{
    /// <summary>
    /// Class contains service method for ToDoList.
    /// </summary>
    public class ToDoListService : IToDoListService
    {
        private readonly IMapper mapper;
        private readonly IToDoListRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoListService"/> class.
        /// </summary>
        /// <param name="mapper">ToDoList mapper.</param>
        /// <param name="repository">ToDoList repository.</param>
        public ToDoListService(IMapper mapper, IToDoListRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        /// <inheritdoc/>
        public Task<int> AddAsync(ToDoListDto toDoListDto)
        {
            if (toDoListDto is null)
            {
                throw new ArgumentNullException(nameof(toDoListDto), "List can not be null.");
            }

            var taskStatusesInDatabase = this.GetByNameInternalAsync(toDoListDto.Name).Result;

            if (taskStatusesInDatabase is not null)
            {
                throw new ArgumentException("List already exist in database.", nameof(toDoListDto));
            }

            return this.AddInternalAsync(toDoListDto);
        }

        /// <inheritdoc/>
        public Task<int> ChangeVisibility(int id, bool visibility)
        {
            var listInDataBase = this.GetByIdInternalAsync(id).Result;

            if (listInDataBase == null)
            {
                throw new ArgumentException("Given list id does not exist in database!", nameof(id));
            }

            listInDataBase.IsHidden = !visibility;

            return this.UpdateInternalAsync(id, listInDataBase);
        }

        /// <inheritdoc/>
        public Task<int> DeleteAsync(int id)
        {
            var itemInDatabaseDto = this.GetByIdInternalAsync(id).Result;

            if (itemInDatabaseDto is null)
            {
                throw new ArgumentException("List with given id does not exist in database.", nameof(id));
            }

            return this.DeleteInternalAsync(id);
        }

        /// <inheritdoc/>
        public Task<int> UpdateAsync(int id, ToDoListDto toDoListDto)
        {
            if (toDoListDto is null)
            {
                throw new ArgumentNullException(nameof(toDoListDto), "List can not be null.");
            }

            var itemInDatabase = this.GetByIdInternalAsync(id).Result;

            if (itemInDatabase is null)
            {
                throw new ArgumentException("List with given id does not exist in database.", nameof(id));
            }

            var itemInDatabaseWithSameName = this.repository.CheckIfExistInDataBaseWithSameNameAsync(toDoListDto.Name).Result;

            if (itemInDatabaseWithSameName is not null)
            {
                throw new ArgumentException("List with given name already exist in database.", nameof(id));
            }

            return this.UpdateInternalAsync(id, toDoListDto);
        }

        /// <inheritdoc/>
        public Task<ToDoListDto> GetByIdAsync(int id)
        {
            var itemDto = this.GetByIdInternalAsync(id);

            if (itemDto.Result is null)
            {
                throw new ArgumentException("List with given id does not exist in database.", nameof(id));
            }

            return itemDto;
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoListDto> GetAll()
        {
            var items = this.repository.GetAll();

            return this.mapper.Map<IEnumerable<ToDoListDto>>(items);
        }

        /// <summary>
        /// Async method to add item.
        /// </summary>
        /// <param name="itemDto">Item to add.</param>
        /// <returns>Added item id.</returns>
        private async Task<int> AddInternalAsync(ToDoListDto itemDto)
        {
            var item = this.mapper.Map<ToDoList>(itemDto);

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
        private async Task<ToDoListDto> GetByIdInternalAsync(int id)
        {
            var itemInDatabase = await this.repository.GetByIdAsync(id);

            return this.mapper.Map<ToDoListDto>(itemInDatabase);
        }

        /// <summary>
        /// Async method to update item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="itemDto">New item data.</param>
        /// <returns>Updated item id.</returns>
        private async Task<int> UpdateInternalAsync(int id, ToDoListDto itemDto)
        {
            var item = this.mapper.Map<ToDoList>(itemDto);

            return await this.repository.UpdateAsync(id, item);
        }

        private async Task<ToDoListDto> GetByNameInternalAsync(string listName)
        {
            var listInDatabase = await this.repository.GetByNameAsync(listName);

            return this.mapper.Map<ToDoListDto>(listInDatabase);
        }
    }
}