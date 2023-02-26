using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToDoApplication.Web.Models.ViewModels
{
    public class CategoriesSelectListVM
    {
        public string SelectedCategory { get; set; } = string.Empty;

        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}
