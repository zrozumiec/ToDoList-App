using AutoMapper;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces;
using ToDoApplication.Application.Services.Base;
using ToDoApplication.Domain.Interfaces;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Application.Services
{
    /// <summary>
    /// Class contains service method for ToDoList.
    /// </summary>
    public class ToDoListService
        : BaseService<ToDoListDto, ToDoList, IToDoListRepository>, IToDoListService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoListService"/> class.
        /// </summary>
        /// <param name="mapper">ToDoList mapper.</param>
        /// <param name="repository">ToDoList repository.</param>
        public ToDoListService(IMapper mapper, IToDoListRepository repository)
            : base(mapper, repository)
        {
        }

        /// <inheritdoc/>
        public override Task<int> AddAsync(ToDoListDto toDoListDto)
        {
            if (toDoListDto is null)
            {
                throw new ArgumentNullException(nameof(toDoListDto), "List can not be null.");
            }

            var taskStatusesInDatabase = this.GetByNameInternalAsync(toDoListDto.Tile);

            if (taskStatusesInDatabase is not null)
            {
                throw new ArgumentException("List already exist in database.", nameof(toDoListDto));
            }

            return this.AddInternalAsync(toDoListDto);
        }

        /// <inheritdoc/>
        public Task<int> ChangeVisibility(int id, bool visibility)
        {
            var listInDataBase = this.GetByIdInternalAsync(id);

            if (listInDataBase == null)
            {
                throw new ArgumentException("Given list id does not exist in database!", nameof(id));
            }

            listInDataBase.Result.IsHidden = !visibility;

            return this.UpdateInternalAsync(id, listInDataBase.Result);
        }

        private async Task<ToDoListDto> GetByNameInternalAsync(string taskCategoryName)
        {
            var taskCategoryInDatabase = await this.Repository.GetByNameAsync(taskCategoryName);

            return this.Mapper.Map<ToDoListDto>(taskCategoryInDatabase);
        }
    }
}