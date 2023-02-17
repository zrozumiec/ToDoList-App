using AutoMapper;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Application.Mapper
{
    /// <summary>
    /// Task category mapper profile.
    /// </summary>
    public class TaskCategoryMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskCategoryMappingProfile"/> class.
        /// </summary>
        public TaskCategoryMappingProfile()
        {
            this.CreateMap<TaskCategory, TaskCategoryDto>()
                .ReverseMap();
        }
    }
}
