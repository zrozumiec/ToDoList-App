using ToDoApplication.Application.DTOs;

namespace ToDoApplication.Web.Models.ViewModels
{
    public class TaskNoteDetailVM
    {
        public TaskNotesDto TaskNote { get; set; } = new TaskNotesDto();
    }
}
