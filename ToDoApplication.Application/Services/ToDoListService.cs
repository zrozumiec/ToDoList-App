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
        private readonly IToDoListRepository listRepository;
        private readonly IToDoTaskService taskService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoListService"/> class.
        /// </summary>
        /// <param name="mapper">ToDoList mapper.</param>
        /// <param name="listRepository">ToDoList repository.</param>
        /// <param name="taskService">ToDoTask service.</param>
        public ToDoListService(
            IMapper mapper,
            IToDoListRepository listRepository,
            IToDoTaskService taskService)
        {
            this.mapper = mapper;
            this.listRepository = listRepository;
            this.taskService = taskService;
        }

        /// <inheritdoc/>
        public Task<int> AddAsync(ToDoListDto toDoListDto)
        {
            if (toDoListDto is null)
            {
                throw new ArgumentNullException(nameof(toDoListDto), "List can not be null.");
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

            var itemInDatabaseWithSameName = this.listRepository.CheckIfExistInDataBaseWithSameNameAsync(toDoListDto.Name).Result;

            if (itemInDatabaseWithSameName is not null && itemInDatabaseWithSameName.Tile != itemInDatabase.Name)
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
            var items = this.listRepository.GetAll();

            return this.mapper.Map<IEnumerable<ToDoListDto>>(items);
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoTaskDto> GetAllUserImportantTasks(string userId)
        {
            var userList = this.GetAllUserLists(userId);

            List<ToDoTaskDto> userTasks = new ();

            foreach (var list in userList)
            {
                userTasks.AddRange(list.Tasks.Where(x => x.Important));
            }

            return this.mapper.Map<IEnumerable<ToDoTaskDto>>(userTasks);
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoTaskDto> GetAllUserDailyTasks(string userId)
        {
            var userList = this.GetAllUserLists(userId);

            List<ToDoTaskDto> userTasks = new ();

            foreach (var list in userList)
            {
                userTasks.AddRange(list.Tasks.Where(x => x.Daily));
            }

            return this.mapper.Map<IEnumerable<ToDoTaskDto>>(userTasks);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ToDoTaskDto>> GetAllUserReminderTasksAsync(string userId)
        {
            var userList = this.GetAllUserLists(userId);

            List<ToDoTaskDto> userTasks = new ();

            foreach (var list in userList)
            {
                foreach (var task in list.Tasks)
                {
                    if (await this.taskService.CheckIfReminderTimeOccursAsync(task.Id))
                    {
                        userTasks.Add(task);
                    }
                }
            }

            return this.mapper.Map<IEnumerable<ToDoTaskDto>>(userTasks);
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoTaskDto> GetAllUserTodaysTasks(string userId)
        {
            var userList = this.GetAllUserLists(userId);

            return this.taskService.GetTaskForToday(userList);
        }

        /// <summary>
        /// Async method to add item.
        /// </summary>
        /// <param name="itemDto">Item to add.</param>
        /// <returns>Added item id.</returns>
        private async Task<int> AddInternalAsync(ToDoListDto itemDto)
        {
            var item = this.mapper.Map<ToDoList>(itemDto);

            return await this.listRepository.AddAsync(item);
        }

        /// <summary>
        /// Async method to delete item.
        /// </summary>
        /// <param name="id">Item id to be deleted.</param>
        /// <returns>Deleted item id.</returns>
        private async Task<int> DeleteInternalAsync(int id)
        {
            return await this.listRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Async method to return item with specified id.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns>Item with specified id.</returns>
        private async Task<ToDoListDto> GetByIdInternalAsync(int id)
        {
            var itemInDatabase = await this.listRepository.GetByIdAsync(id);

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

            return await this.listRepository.UpdateAsync(id, item);
        }

        /// <summary>
        /// Gets all user lists.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>User lists.</returns>
        private IEnumerable<ToDoListDto> GetAllUserLists(string userId)
        {
            var userList = this.listRepository.GetAll()
                    .Where(x => x.UserId == userId)
                    .Where(x => x.Tile != "Important" && x.Tile != "Today" && x.Tile != "Daily");

            return this.mapper.Map<IEnumerable<ToDoListDto>>(userList);
        }
    }
}