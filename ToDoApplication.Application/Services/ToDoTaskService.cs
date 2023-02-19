using AutoMapper;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces;
using ToDoApplication.Application.Services.Base;
using ToDoApplication.Domain.Interfaces;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Application.Services
{
    /// <summary>
    /// Class contains service method for task.
    /// </summary>
    public class ToDoTaskService
        : BaseService<ToDoTaskDto, ToDoTask, IToDoTaskRepository>, IToDoTaskService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoTaskService"/> class.
        /// </summary>
        /// <param name="mapper">Task mapper.</param>
        /// <param name="repository">Task repository.</param>
        public ToDoTaskService(IMapper mapper, IToDoTaskRepository repository)
            : base(mapper, repository)
        {
        }

        /// <inheritdoc/>
        public Task<int> TurnOnOffReminder(int id, bool turnOnOff)
        {
            var taskInDataBase = this.GetByIdInternalAsync(id);

            if (taskInDataBase == null)
            {
                throw new ArgumentException("Given task id does not exist in database!", nameof(id));
            }

            taskInDataBase.Result.Reminder = turnOnOff;

            return this.UpdateInternalAsync(id, taskInDataBase.Result);
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoTaskDto> GetDailyTasks()
        {
            var dailyTask = this.Repository.GetAll().Where(x => x.Daily);

            return this.Mapper.Map<IEnumerable<ToDoTaskDto>>(dailyTask);
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoTaskDto> GetImportantTasks()
        {
            var importantTask = this.Repository.GetAll().Where(x => x.Important);

            return this.Mapper.Map<IEnumerable<ToDoTaskDto>>(importantTask);
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoTaskDto> GetTaskForToday()
        {
            var dateNow = DateTimeOffset.Now;
            var tasksForToday = this.Repository.GetAll().Where(x => CheckIfDueDateOccurs(dateNow, x.DueDate));

            return this.Mapper.Map<IEnumerable<ToDoTaskDto>>(tasksForToday);
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoTaskDto> GetCompletedTasks()
        {
            var completedTask = this.Repository.GetAll().Where(x => x.IsCompleted);

            return this.Mapper.Map<IEnumerable<ToDoTaskDto>>(completedTask);
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoTaskDto> GetUncompletedTasks()
        {
            var uncompletedTask = this.Repository.GetAll().Where(x => !x.IsCompleted);

            return this.Mapper.Map<IEnumerable<ToDoTaskDto>>(uncompletedTask);
        }

        private static bool CheckIfDueDateOccurs(DateTimeOffset date1, DateTimeOffset date2)
        {
            return (date1.Year == date2.Year) && (date2.DayOfYear == date1.DayOfYear);
        }
    }
}