using Microsoft.AspNetCore.Mvc.Rendering;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Web.Models.ViewModels;

namespace ToDoApplication.Web.Models
{
    public static class FillSetectedList
    {
        public static void FillTaskSelectedLists(
            CreateTaskVM task,
            IEnumerable<TaskCategoryDto>? categories,
            IEnumerable<TaskPriorityDto>? priorities,
            IEnumerable<TaskStatusesDto>? statuses)
        {
            if (categories is not null)
            {
                foreach (var c in categories)
                {
                    task.CategoriesSelectedList.Categories.Add(new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
                }
            }

            if (priorities is not null)
            {
                foreach (var p in priorities)
                {
                    task.PrioritiesSelectedList.Priorities.Add(new SelectListItem { Text = p.Name, Value = p.Id.ToString() });
                }
            }

            if (statuses is not null)
            {
                foreach (var s in statuses)
                {
                    task.StatusesSelectedList.Statuses.Add(new SelectListItem { Text = s.Name, Value = s.Id.ToString() });
                }
            }
        }
    }
}
