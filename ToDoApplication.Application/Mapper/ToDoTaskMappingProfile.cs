using AutoMapper;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Application.Mapper
{
    /// <summary>
    /// ToDoTask mapper profile.
    /// </summary>
    public class ToDoTaskMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoTaskMappingProfile"/> class.
        /// </summary>
        public ToDoTaskMappingProfile()
        {
            this.CreateMap<ToDoTask, ToDoTaskDto>()
                .ReverseMap();
        }
    }
}
