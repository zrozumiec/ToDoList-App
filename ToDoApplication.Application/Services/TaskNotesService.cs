using AutoMapper;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces;
using ToDoApplication.Application.Services.Base;
using ToDoApplication.Domain.Interfaces;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Application.Services
{
    /// <summary>
    /// Class contains service method for task notes.
    /// </summary>
    public class TaskNotesService
        : BaseService<TaskNotesDto, TaskNotes, ITaskNotesRepository>, ITaskNotesService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskNotesService"/> class.
        /// </summary>
        /// <param name="mapper">Task notes mapper.</param>
        /// <param name="repository">Task notes repository.</param>
        public TaskNotesService(IMapper mapper, ITaskNotesRepository repository)
            : base(mapper, repository)
        {
        }
    }
}