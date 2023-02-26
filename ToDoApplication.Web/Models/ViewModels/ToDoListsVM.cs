using ToDoApplication.Application.DTOs;

namespace ToDoApplication.Web.Models.ViewModels
{
    public class ToDoListsVM
    {
        public bool ShowAll { get; set; }

        public IEnumerable<ToDoListDto> ToDoLists { get; set; } = new List<ToDoListDto>();
    }
}
