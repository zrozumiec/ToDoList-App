using AutoMapper;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces;
using ToDoApplication.Domain.Interfaces;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Application.Services
{
    /// <summary>
    /// Class contains service method for task.
    /// </summary>
    public class ToDoTaskService : IToDoTaskService
    {
        private readonly IMapper mapper;
        private readonly IToDoTaskRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoTaskService"/> class.
        /// </summary>
        /// <param name="mapper">Task mapper.</param>
        /// <param name="repository">Task repository.</param>
        public ToDoTaskService(IMapper mapper, IToDoTaskRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        /// <inheritdoc/>
        public Task<int> AddAsync(ToDoTaskDto taskDto)
        {
            if (taskDto is null)
            {
                throw new ArgumentNullException(nameof(taskDto), "Task to add can not be null.");
            }

            return this.AddInternalAsync(taskDto);
        }

        /// <inheritdoc/>
        public Task<int> DeleteAsync(int id)
        {
            var itemInDatabaseDto = this.GetByIdInternalAsync(id).Result;

            if (itemInDatabaseDto is null)
            {
                throw new ArgumentException("Task with given id does not exist in database.", nameof(id));
            }

            return this.DeleteInternalAsync(id);
        }

        /// <inheritdoc/>
        public Task<int> UpdateAsync(int id, ToDoTaskDto taskDto)
        {
            if (taskDto is null)
            {
                throw new ArgumentNullException(nameof(taskDto), "Task can not be null.");
            }

            var itemInDatabase = this.GetByIdInternalAsync(id).Result;

            if (itemInDatabase is null)
            {
                throw new ArgumentException("Task with given id does not exist in database.", nameof(id));
            }

            return this.UpdateInternalAsync(id, taskDto);
        }

        /// <inheritdoc/>
        public Task<ToDoTaskDto> GetByIdAsync(int id)
        {
            var itemDto = this.GetByIdInternalAsync(id);

            if (itemDto.Result is null)
            {
                throw new ArgumentException("Task with given id does not exist in database.", nameof(id));
            }

            return itemDto;
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoTaskDto> GetAll(int listId)
        {
            var items = this.repository.GetAll(listId);

            return this.mapper.Map<IEnumerable<ToDoTaskDto>>(items);
        }

        /// <inheritdoc/>
        public Task<int> TurnOnOffReminderAsync(int id, bool turnOnOff)
        {
            var taskInDataBase = this.GetByIdInternalAsync(id).Result;

            if (taskInDataBase == null)
            {
                throw new ArgumentException("Given task id does not exist in database!", nameof(id));
            }

            taskInDataBase.Reminder = turnOnOff;

            return this.UpdateInternalAsync(id, taskInDataBase);
        }

        /// <inheritdoc/>
        public Task<int> SetReminderTimeAsync(int id, DateTimeOffset reminderDate)
        {
            var taskInDataBase = this.GetByIdInternalAsync(id).Result;

            if (taskInDataBase == null)
            {
                throw new ArgumentException("Given task id does not exist in database!", nameof(id));
            }

            if (reminderDate <= taskInDataBase.CreationDate)
            {
                throw new ArgumentException("Reminder date can not be less than create date.", nameof(reminderDate));
            }

            taskInDataBase.ReminderDate = reminderDate;

            return this.UpdateInternalAsync(id, taskInDataBase);
        }

        /// <inheritdoc/>
        public async Task<bool> CheckIfReminderTimeOccursAsync(int id)
        {
            var taskInDataBase = await this.GetByIdInternalAsync(id);

            if (CheckIfReminderDateOccurs(taskInDataBase.ReminderDate, DateTimeOffset.Now) >= 0
                && taskInDataBase.Reminder)
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoTaskDto> GetDailyTasks()
        {
            var dailyTask = this.repository.GetAll().Where(x => x.Daily);

            return this.mapper.Map<IEnumerable<ToDoTaskDto>>(dailyTask);
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoTaskDto> GetImportantTasks()
        {
            var importantTask = this.repository.GetAll().Where(x => x.Important);

            return this.mapper.Map<IEnumerable<ToDoTaskDto>>(importantTask);
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoTaskDto> GetTaskForToday()
        {
            var dateNow = DateTimeOffset.Now;
            var tasksForToday = this.repository.GetAll().Where(x => CheckIfDueDateOccurs(dateNow, x.DueDate));

            return this.mapper.Map<IEnumerable<ToDoTaskDto>>(tasksForToday);
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoTaskDto> GetCompletedTasks()
        {
            var completedTask = this.repository.GetAll().Where(x => x.IsCompleted);

            return this.mapper.Map<IEnumerable<ToDoTaskDto>>(completedTask);
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoTaskDto> GetUncompletedTasks()
        {
            var uncompletedTask = this.repository.GetAll().Where(x => !x.IsCompleted);

            return this.mapper.Map<IEnumerable<ToDoTaskDto>>(uncompletedTask);
        }

        private static bool CheckIfDueDateOccurs(DateTimeOffset date1, DateTimeOffset date2)
        {
            return (date1.Year == date2.Year) && (date2.DayOfYear == date1.DayOfYear);
        }

        private static int CheckIfReminderDateOccurs(DateTimeOffset reminderDate, DateTimeOffset dateTimeNow)
        {
            return DateTime.Compare(dateTimeNow.DateTime, reminderDate.DateTime);
        }

        /// <summary>
        /// Async method to add item.
        /// </summary>
        /// <param name="itemDto">Item to add.</param>
        /// <returns>Added item id.</returns>
        private async Task<int> AddInternalAsync(ToDoTaskDto itemDto)
        {
            var item = this.mapper.Map<ToDoTask>(itemDto);

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
        private async Task<ToDoTaskDto> GetByIdInternalAsync(int id)
        {
            var itemInDatabase = await this.repository.GetByIdAsync(id);

            return this.mapper.Map<ToDoTaskDto>(itemInDatabase);
        }

        /// <summary>
        /// Async method to update item.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="itemDto">New item data.</param>
        /// <returns>Updated item id.</returns>
        private async Task<int> UpdateInternalAsync(int id, ToDoTaskDto itemDto)
        {
            var item = this.mapper.Map<ToDoTask>(itemDto);

            return await this.repository.UpdateAsync(id, item);
        }
    }
}