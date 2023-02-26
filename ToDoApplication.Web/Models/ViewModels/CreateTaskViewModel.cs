using Microsoft.AspNetCore.Mvc.Rendering;
using ToDoApplication.Application.DTOs;

namespace ToDoApplication.Web.Models.ViewModels
{
    public class CreateTaskViewModel
    {
        public int ListId { get; set; }

        public ToDoTaskDto ToDoTask { get; set; } = new ToDoTaskDto();

        public CategoriesSelectListVM CategoriesSelectedList { get; set; } = new CategoriesSelectListVM();

        public PrioritySelectListVM PrioritiesSelectedList { get; set; } = new PrioritySelectListVM();

        public StatusesSelectListVM StatusesSelectedList { get; set; } = new StatusesSelectListVM();
    }
}
