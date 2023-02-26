using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces;

namespace ToDoApplication.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TaskStatusController : Controller
    {
        private readonly ITaskStatusesService statusService;
        private readonly IToDoTaskService taskService;
        private readonly IValidator<TaskStatusesDto> validator;

        public TaskStatusController(
            ITaskStatusesService statusService,
            IToDoTaskService taskService,
            IValidator<TaskStatusesDto> validator)
        {
            this.statusService = statusService;
            this.taskService = taskService;
            this.validator = validator;
        }

        public IActionResult Index()
        {
            var taskStatuses = this.statusService.GetAll();

            return this.View(taskStatuses);
        }

        [Route("TaskStatuses/Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var taskStatus = await this.statusService.GetByIdAsync(id);
            taskStatus.Tasks = this.taskService.GetAll().Where(x => x.StatusId == id);

            return this.View(taskStatus);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return this.View(new TaskStatusesDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TaskStatusesDto status)
        {
            var result = await this.validator.ValidateAsync(status);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return this.View("Add", status);
            }

            var statusId = await this.statusService.AddAsync(status);

            return this.RedirectToAction("Details", new { id = statusId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var taskStatus = await this.statusService.GetByIdAsync(id);

            return this.View(taskStatus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskStatusesDto status)
        {
            var result = await this.validator.ValidateAsync(status);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return this.View("Edit", status);
            }

            var statusId = await this.statusService.UpdateAsync(status.Id, status);

            return this.RedirectToAction("Details", new { id = statusId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.statusService.DeleteAsync(id);

            return this.RedirectToAction("Index");
        }
    }
}
