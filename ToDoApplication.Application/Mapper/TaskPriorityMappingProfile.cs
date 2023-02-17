using AutoMapper;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Application.Mapper
{
    /// <summary>
    /// Task priority mapper profile.
    /// </summary>
    public class TaskPriorityMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskPriorityMappingProfile"/> class.
        /// </summary>
        public TaskPriorityMappingProfile()
        {
            this.CreateMap<TaskPriority, TaskPriorityDto>()
                .ReverseMap();
        }
    }
}
