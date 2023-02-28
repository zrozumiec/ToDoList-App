using AutoMapper;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces;
using ToDoApplication.Domain.Interfaces;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Application.Services
{
    /// <summary>
    /// Class contains service method for task notes.
    /// </summary>
    public class TaskNotesService : ITaskNotesService
    {
        private readonly IMapper mapper;
        private readonly ITaskNotesRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskNotesService"/> class.
        /// </summary>
        /// <param name="mapper">Task notes mapper.</param>
        /// <param name="repository">Task notes repository.</param>
        public TaskNotesService(IMapper mapper, ITaskNotesRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        /// <inheritdoc/>
        public Task<int> AddAsync(TaskNotesDto taskNoteDto)
        {
            if (taskNoteDto is null)
            {
                throw new ArgumentNullException(nameof(taskNoteDto), "Note to add can not be null.");
            }

            return this.AddInternalAsync(taskNoteDto);
        }

        /// <inheritdoc/>
        public Task<int> DeleteAsync(int id)
        {
            var itemInDatabaseDto = this.GetByIdInternalAsync(id).Result;

            if (itemInDatabaseDto is null)
            {
                throw new ArgumentException("Note with given id does not exist in database.", nameof(id));
            }

            return this.DeleteInternalAsync(id);
        }

        /// <inheritdoc/>
        public Task<int> UpdateAsync(int id, TaskNotesDto taskNoteDto)
        {
            if (taskNoteDto is null)
            {
                throw new ArgumentNullException(nameof(taskNoteDto), "Note can not be null.");
            }

            var itemInDatabase = this.GetByIdInternalAsync(id).Result;

            if (itemInDatabase is null)
            {
                throw new ArgumentException("Note with given id does not exist in database.", nameof(id));
            }

            return this.UpdateInternalAsync(id, taskNoteDto);
        }

        /// <inheritdoc/>
        public Task<TaskNotesDto> GetByIdAsync(int id)
        {
            var itemDto = this.GetByIdInternalAsync(id);

            if (itemDto.Result is null)
            {
                throw new ArgumentException("Note with given id does not exist in database.", nameof(id));
            }

            return itemDto;
        }

        /// <inheritdoc/>
        public IEnumerable<TaskNotesDto> GetAll(int id)
        {
            var items = this.repository.GetAll(id);

            return this.mapper.Map<IEnumerable<TaskNotesDto>>(items);
        }

        /// <summary>
        /// Async method to add item.
        /// </summary>
        /// <param name="itemDto">Item to add.</param>
        /// <returns>Added item id.</returns>
        private async Task<int> AddInternalAsync(TaskNotesDto itemDto)
        {
            var item = this.mapper.Map<TaskNotes>(itemDto);

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
        private async Task<TaskNotesDto> GetByIdInternalAsync(int id)
        {
            var itemInDatabase = await this.repository.GetByIdAsync(id);

            return this.mapper.Map<TaskNotesDto>(itemInDatabase);
        }

        /// <summary>
        /// Async method to update item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="itemDto">New item data.</param>
        /// <returns>Updated item id.</returns>
        private async Task<int> UpdateInternalAsync(int id, TaskNotesDto itemDto)
        {
            var item = this.mapper.Map<TaskNotes>(itemDto);

            return await this.repository.UpdateAsync(id, item);
        }
    }
}