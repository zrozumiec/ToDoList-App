using ToDoApplication.Application.Interfaces;
using ToDoApplication.Web.Models.ViewModels;

namespace ToDoApplication.Web.Models
{
    public static class CountDefaultListItems
    {
        public static void FillNumberOfTasks(string userId, ToDoListsVM lists, IToDoListService toDoListService)
        {
            var importantTasks = lists.ToDoLists.FirstOrDefault(x => x.Name == "Important");
            var dailyTasks = lists.ToDoLists.FirstOrDefault(x => x.Name == "Daily");
            var todayTasks = lists.ToDoLists.FirstOrDefault(x => x.Name == "Today");

            if (importantTasks is not null)
            {
                importantTasks.NumberOfTasks = toDoListService.GetAllUserImportantTasks(userId).Count();
            }

            if (dailyTasks is not null)
            {
                dailyTasks.NumberOfTasks = toDoListService.GetAllUserDailyTasks(userId).Count();
            }

            if (todayTasks is not null)
            {
                todayTasks.NumberOfTasks = toDoListService.GetAllUserTodaysTasks(userId).Count();
            }
        }
    }
}
