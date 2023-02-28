using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces;
using ToDoApplication.Domain.Models;
using ToDoApplication.Web.Models;
using ToDoApplication.Web.Models.ViewModels;

namespace ToDoApplication.Web.Controllers
{
    [Authorize]
    public class ToDoTaskController : Controller
    {
        private readonly IToDoTaskService toDoTaskService;
        private readonly ITaskStatusesService statusService;
        private readonly ITaskCategoryService categoryService;
        private readonly ITaskPriorityService priorityService;
        private readonly IToDoListService toDoListService;
        private readonly ITaskNotesService taskNotesService;
        private readonly IValidator<ToDoTaskDto> validator;
        private readonly UserManager<ApplicationUser> userManager;

        public ToDoTaskController(
            IToDoTaskService toDoTaskService,
            ITaskStatusesService statusService,
            ITaskCategoryService categoryService,
            ITaskPriorityService priorityService,
            IToDoListService toDoListService,
            ITaskNotesService taskNotesService,
            IValidator<ToDoTaskDto> validator,
            UserManager<ApplicationUser> userManager)
        {
            this.toDoTaskService = toDoTaskService;
            this.statusService = statusService;
            this.categoryService = categoryService;
            this.priorityService = priorityService;
            this.toDoListService = toDoListService;
            this.taskNotesService = taskNotesService;
            this.validator = validator;
            this.userManager = userManager;
        }

        [Route("ToDoList/{listId:int}/Tasks/{hide?}")]
        public async Task<IActionResult> Index(int listId, bool showAll)
        {
            var list = await this.toDoListService.GetByIdAsync(listId);
            var userId = this.userManager.GetUserId(this.HttpContext.User);

            var listWithTasks = new ToDoListWithTasksVM();

            listWithTasks = list.Name switch
            {
                "Important" => new ToDoListWithTasksVM
                {
                    ListId = listId,
                    ShowAll = showAll,
                    ToDoTasks = this.toDoListService.GetAllUserImportantTasks(userId)
                },
                "Daily" => new ToDoListWithTasksVM
                {
                    ListId = listId,
                    ShowAll = showAll,
                    ToDoTasks = this.toDoListService.GetAllUserDailyTasks(userId)
                },
                "Today" => new ToDoListWithTasksVM
                {
                    ListId = listId,
                    ShowAll = showAll,
                    ToDoTasks = this.toDoListService.GetAllUserTodaysTasks(userId)
                },
                _ => new ToDoListWithTasksVM
                {
                    ListId = listId,
                    ShowAll = showAll,
                    ToDoTasks = this.toDoTaskService.GetAll(listId)
                },
            };
            if (!showAll)
            {
                listWithTasks.ToDoTasks = listWithTasks.ToDoTasks.Where(x => !x.IsCompleted);
            }

            foreach (var task in listWithTasks.ToDoTasks)
            {
                task.List = await this.toDoListService.GetByIdAsync(task.ListId);
                task.Category = await this.categoryService.GetByIdAsync(task.CategoryId);
                task.Status = await this.statusService.GetByIdAsync(task.StatusId);
                task.Priority = await this.priorityService.GetByIdAsync(task.PriorityId);
                var listOfNotes = this.taskNotesService.GetAll(task.Id);
                task.NumberOfNotes = listOfNotes.Count();
            }

            return this.View(listWithTasks);
        }

        [Route("ToDoList/{listId:int}/Task/Details/{id:int}")]
        public async Task<IActionResult> Details(int id, int listId)
        {
            var task = new TaskDetailVM
            {
                ListId = listId,
                ToDoTask = await this.toDoTaskService.GetByIdAsync(id)
            };
            task.ToDoTask.Status = await this.statusService.GetByIdAsync(task.ToDoTask.StatusId);
            task.ToDoTask.Category = await this.categoryService.GetByIdAsync(task.ToDoTask.CategoryId);
            task.ToDoTask.Priority = await this.priorityService.GetByIdAsync(task.ToDoTask.PriorityId);
            task.ToDoTask.List = await this.toDoListService.GetByIdAsync(task.ToDoTask.ListId);

            return this.View(task);
        }

        [Route("ToDoList/{listId:int}/Tasks/Add")]
        [HttpGet]
        public IActionResult Add(int listId)
        {
            var categories = this.categoryService.GetAll();
            var priorities = this.priorityService.GetAll();
            var createTaskViewModel = new CreateTaskVM
            {
                ListId = listId
            };

            FillSetectedList.FillTaskSelectedLists(createTaskViewModel, categories, priorities, null);

            return this.View(createTaskViewModel);
        }

        [Route("ToDoList/{listId:int}/Tasks/Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int listId, CreateTaskVM task)
        {
            var parsePriority = int.TryParse(task.PrioritiesSelectedList.SelectedPriority, out var priorityId);
            var parseCategory = int.TryParse(task.CategoriesSelectedList.SelectedCategory, out var categoryId);

            if (!parsePriority || !parseCategory)
            {
                return this.RedirectToAction("Add", new { listId = task.ListId });
            }

            ToDoTaskDto toDoTask = task.ToDoTask;

            toDoTask.ListId = listId;
            toDoTask.PriorityId = priorityId;
            toDoTask.CategoryId = categoryId;
            toDoTask.StatusId = 1;

            toDoTask.List = null!;
            toDoTask.Priority = null!;
            toDoTask.Category = null!;
            toDoTask.Status = null!;

            toDoTask.CreationDate = DateTimeOffset.Now;

            var result = await this.validator.ValidateAsync(task.ToDoTask);

            if (!result.IsValid)
            {
                var categories = this.categoryService.GetAll();
                var priorities = this.priorityService.GetAll();

                FillSetectedList.FillTaskSelectedLists(task, categories, priorities, null);

                result.AddToModelState(this.ModelState);

                return this.View("Add", task);
            }

            var taskId = await this.toDoTaskService.AddAsync(task.ToDoTask);

            return this.RedirectToAction("Details", new { id = taskId, listId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, int listId)
        {
            var createTaskViewModel = new CreateTaskVM();
            var categories = this.categoryService.GetAll();
            var priorities = this.priorityService.GetAll();
            var statuses = this.statusService.GetAll();

            FillSetectedList.FillTaskSelectedLists(createTaskViewModel, categories, priorities, statuses);

            createTaskViewModel.ToDoTask = await this.toDoTaskService.GetByIdAsync(id);
            createTaskViewModel.ListId = listId;
            createTaskViewModel.ToDoTask.Reminder = false;

            return this.View(createTaskViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateTaskVM task)
        {
            var parsePriority = int.TryParse(task.PrioritiesSelectedList.SelectedPriority, out var priorityId);
            var parseCategory = int.TryParse(task.CategoriesSelectedList.SelectedCategory, out var categoryId);
            var parseStatusy = int.TryParse(task.StatusesSelectedList.SelectedStatus, out var statusId);

            if (!parsePriority || !parseCategory || !parseStatusy)
            {
                return this.View("Edit", task);
            }

            ToDoTaskDto toDoTask = task.ToDoTask;

            toDoTask.ListId = task.ListId;
            toDoTask.PriorityId = priorityId;
            toDoTask.CategoryId = categoryId;
            toDoTask.StatusId = statusId;
            toDoTask.IsCompleted = this.statusService.GetByIdAsync(statusId).Result.Name == "Completed";

            toDoTask.List = null!;
            toDoTask.Priority = null!;
            toDoTask.Category = null!;
            toDoTask.Status = null!;

            var result = await this.validator.ValidateAsync(task.ToDoTask);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return this.View("Edit", task);
            }

            var taskId = await this.toDoTaskService.UpdateAsync(task.ToDoTask.Id, task.ToDoTask);

            return this.RedirectToAction("Details", new { id = taskId, listId = task.ListId });
        }

        public async Task<IActionResult> Delete(int id, int listId)
        {
            await this.toDoTaskService.DeleteAsync(id);

            return this.RedirectToAction("Index", new { listId });
        }
    }
}
