using AutoMapper;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Application.Mapper
{
    /// <summary>
    /// Task notes mapper profile.
    /// </summary>
    public class TaskNotesMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskNotesMappingProfile"/> class.
        /// </summary>
        public TaskNotesMappingProfile()
        {
            this.CreateMap<TaskNotes, TaskNotesDto>()
                .ForMember(m => m.Name, d => d.MapFrom(s => s.Description))
                .ReverseMap();
        }
    }
}
