using AutoMapper;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Application.Mapper
{
    /// <summary>
    /// Task statuses mapper profile.
    /// </summary>
    public class TaskStatusesMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskStatusesMappingProfile"/> class.
        /// </summary>
        public TaskStatusesMappingProfile()
        {
            this.CreateMap<TaskStatuses, TaskStatusesDto>()
                .ReverseMap();
        }
    }
}
