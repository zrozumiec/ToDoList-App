using ToDoApplication.Application.DTOs;

namespace ToDoApplication.Web.Models.ViewModels
{
    public class TaskWithNotesVM
    {
        public int TaskId { get; set; }
        public bool ShowAll { get; set; }
        public IEnumerable<TaskNotesDto> TaskNotes { get; set; } = new List<TaskNotesDto>();
    }
}
