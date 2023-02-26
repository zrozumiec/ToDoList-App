using ToDoApplication.Application.DTOs;

namespace ToDoApplication.Web.Models.ViewModels
{
    public class TaskDetailVM
    {
        public int ListId { get; set; }

        public ToDoTaskDto ToDoTask { get; set; } = new ToDoTaskDto();
    }
}
