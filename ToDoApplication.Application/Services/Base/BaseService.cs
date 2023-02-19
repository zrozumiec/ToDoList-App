using AutoMapper;
using ToDoApplication.Application.DTOs.Base;
using ToDoApplication.Application.Interfaces.Base;
using ToDoApplication.Domain.Interfaces.Base;
using ToDoApplication.Domain.Models.Base;

namespace ToDoApplication.Application.Services.Base
{
    /// <summary>
    /// Base class for other services.
    /// </summary>
    /// <typeparam name="TDto">Dto item type.</typeparam>
    /// <typeparam name="T">Item type.</typeparam>
    /// <typeparam name="TI">Item repository.</typeparam>
    public abstract class BaseService<TDto, T, TI> : IBaseService<TDto>
        where TDto : BaseDto
        where T : BaseEntity
        where TI : IBaseRepository<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService{TDto, T, TI}"/> class.
        /// </summary>
        /// <param name="mapper">Mapper.</param>
        /// <param name="repository">Repository.</param>
        protected BaseService(IMapper mapper, TI repository)
        {
            this.Mapper = mapper;
            this.Repository = repository;
        }

        /// <summary>
        /// Gets mapper.
        /// </summary>
        public IMapper Mapper { get; }

        /// <summary>
        /// Gets repository.
        /// </summary>
        public TI Repository { get; }

        /// <inheritdoc/>
        public virtual Task<int> AddAsync(TDto itemDto)
        {
            if (itemDto is null)
            {
                throw new ArgumentNullException(nameof(itemDto), "Item to add can not be null.");
            }

            return this.AddInternalAsync(itemDto);
        }

        /// <inheritdoc/>
        public Task<int> DeleteAsync(int id)
        {
            var itemInDatabaseDto = this.GetByIdInternalAsync(id).Result;

            if (itemInDatabaseDto is null)
            {
                throw new ArgumentException("Item with given id does not exist in database.", nameof(id));
            }

            return this.DeleteInternalAsync(id);
        }

        /// <inheritdoc/>
        public Task<int> UpdateAsync(int id, TDto itemDto)
        {
            if (itemDto is null)
            {
                throw new ArgumentNullException(nameof(itemDto), "Item can not be null.");
            }

            var itemInDatabase = this.GetByIdInternalAsync(id).Result;

            if (itemInDatabase is null)
            {
                throw new ArgumentException("Item with given id does not exist in database.", nameof(id));
            }

            var itemInDatabaseWithSameName = this.GetAll().FirstOrDefault(x => x.Name.ToLower() == itemDto.Name.ToLower());

            if (itemInDatabaseWithSameName is not null)
            {
                throw new ArgumentException("Item with given name already exist in database.", nameof(id));
            }

            return this.UpdateInternalAsync(id, itemDto);
        }

        /// <inheritdoc/>
        public Task<TDto> GetByIdAsync(int id)
        {
            var itemDto = this.GetByIdInternalAsync(id);

            if (itemDto.Result is null)
            {
                throw new ArgumentException("Item with given id does not exist in database.", nameof(id));
            }

            return itemDto;
        }

        /// <inheritdoc/>
        public IEnumerable<TDto> GetAll()
        {
            var items = this.Repository.GetAll();

            return this.Mapper.Map<IEnumerable<TDto>>(items);
        }

        /// <summary>
        /// Async method to add item.
        /// </summary>
        /// <param name="itemDto">Item to add.</param>
        /// <returns>Added item id.</returns>
        protected async Task<int> AddInternalAsync(TDto itemDto)
        {
            var item = this.Mapper.Map<T>(itemDto);

            return await this.Repository.AddAsync(item);
        }

        /// <summary>
        /// Async method to delete item.
        /// </summary>
        /// <param name="id">Item id to be deleted.</param>
        /// <returns>Deleted item id.</returns>
        protected async Task<int> DeleteInternalAsync(int id)
        {
            return await this.Repository.DeleteAsync(id);
        }

        /// <summary>
        /// Async method to return item with specified id.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns>Item with specified id.</returns>
        protected async Task<TDto> GetByIdInternalAsync(int id)
        {
            var itemInDatabase = await this.Repository.GetByIdAsync(id);

            return this.Mapper.Map<TDto>(itemInDatabase);
        }

        /// <summary>
        /// Async method to update item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="itemDto">New item data.</param>
        /// <returns>Updated item id.</returns>
        protected async Task<int> UpdateInternalAsync(int id, TDto itemDto)
        {
            var item = this.Mapper.Map<T>(itemDto);

            return await this.Repository.UpdateAsync(id, item);
        }
    }
}
