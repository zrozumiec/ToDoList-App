using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoApplication.Application.Interfaces;
using ToDoApplication.Domain.Models;
using ToDoApplication.Web.Models.ViewModels;

namespace ToDoApplication.Web.Models
{
    public class Reminder : Controller
    {
        private readonly IToDoListService toDoListService;
        private readonly UserManager<ApplicationUser> userManager;

        public Reminder(
            IToDoListService toDoListService,
            UserManager<ApplicationUser> userManager)
        {
            this.toDoListService = toDoListService;
            this.userManager = userManager;
        }

        public async Task<(bool, ReminderVM)> CheckIfRemindTask(string? userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return (false, null!);
            }

            var userId = this.userManager.Users?.FirstOrDefault(x => x.UserName == userName)?.Id;
            var tasks = userId is null ? null : await this.toDoListService.GetAllUserReminderTasksAsync(userId);

            if (tasks == null || !tasks.Any())
            {
                return (false, null!);
            }

            var reminderVM = new ReminderVM()
            {
                TaskId = tasks.Select(x => x.Id).ToList(),
                ListId = tasks.Select(x => x.ListId).ToList(),
                Name = tasks.Select(x => x.Name).ToList(),
            };

            return (true, reminderVM);
        }
    }
}
