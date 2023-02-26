using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToDoApplication.Web.Models.ViewModels
{
    public class PrioritySelectListVM
    {
        public string SelectedPriority { get; set; } = string.Empty;

        public List<SelectListItem> Priorities { get; set; } = new List<SelectListItem>();
    }
}
