using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TaskPriorityController : Controller
    {
        private readonly ITaskPriorityService priorityService;
        private readonly IToDoTaskService taskService;
        private readonly IValidator<TaskPriorityDto> validator;

        public TaskPriorityController(
            ITaskPriorityService priorityService,
            IToDoTaskService taskService,
            IValidator<TaskPriorityDto> validator)
        {
            this.priorityService = priorityService;
            this.taskService = taskService;
            this.validator = validator;
        }

        public IActionResult Index()
        {
            var taskPriority = this.priorityService.GetAll();

            return this.View(taskPriority);
        }

        [Route("TaskPriority/Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var taskPriority = await this.priorityService.GetByIdAsync(id);
            taskPriority.Tasks = this.taskService.GetAll().Where(x => x.StatusId == id);

            return this.View(taskPriority);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return this.View(new TaskPriorityDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TaskPriorityDto priority)
        {
            var result = await this.validator.ValidateAsync(priority);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return this.View("Add", priority);
            }

            var priorityId = await this.priorityService.AddAsync(priority);

            return this.RedirectToAction("Details", new { id = priorityId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var taskPriority = await this.priorityService.GetByIdAsync(id);

            return this.View(taskPriority);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskPriorityDto priority)
        {
            var result = await this.validator.ValidateAsync(priority);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return this.View("Edit", priority);
            }

            var priorityId = await this.priorityService.UpdateAsync(priority.Id, priority);

            return this.RedirectToAction("Details", new { id = priorityId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.priorityService.DeleteAsync(id);

            return this.RedirectToAction("Index");
        }
    }
}
