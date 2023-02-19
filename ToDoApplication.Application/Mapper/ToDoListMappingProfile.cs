using AutoMapper;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Application.Mapper
{
    /// <summary>
    /// ToDoList mapper profile.
    /// </summary>
    public class ToDoListMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoListMappingProfile"/> class.
        /// </summary>
        public ToDoListMappingProfile()
        {
            this.CreateMap<ToDoList, ToDoListDto>()
                .ForMember(m => m.Name, d => d.MapFrom(s => s.Tile))
                .ForMember(m => m.NumberOfTasks, d => d.MapFrom(s => s.Tasks.Count))
                .ReverseMap();
        }
    }
}
