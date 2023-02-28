using ToDoApplication.Application.DTOs;

namespace ToDoApplication.Web.Models.ViewModels
{
    public class ToDoListWithTasksVM
    {
        public int ListId { get; set; }

        public bool ShowAll { get; set; }

        public IEnumerable<ToDoTaskDto> ToDoTasks { get; set; } = new List<ToDoTaskDto>();
    }
}
