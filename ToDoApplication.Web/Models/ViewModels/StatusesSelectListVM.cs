using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToDoApplication.Web.Models.ViewModels
{
    public class StatusesSelectListVM
    {
        public string SelectedStatus { get; set; } = string.Empty;

        public List<SelectListItem> Statuses { get; set; } = new List<SelectListItem>();
    }
}
