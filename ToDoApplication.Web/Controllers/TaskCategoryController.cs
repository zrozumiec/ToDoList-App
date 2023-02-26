using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces;

namespace ToDoApplication.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TaskCategoryController : Controller
    {
        private readonly ITaskCategoryService taskCategoryService;
        private readonly IToDoTaskService taskService;
        private readonly IValidator<TaskCategoryDto> validator;

        public TaskCategoryController(
            ITaskCategoryService taskCategoryService,
            IToDoTaskService taskService,
            IValidator<TaskCategoryDto> validator)
        {
            this.taskCategoryService = taskCategoryService;
            this.taskService = taskService;
            this.validator = validator;
        }

        public IActionResult Index()
        {
            var taskCategories = this.taskCategoryService.GetAll();

            return this.View(taskCategories);
        }

        [Route("TaskCategory/Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var taskCategory = await this.taskCategoryService.GetByIdAsync(id);
            taskCategory.Tasks = this.taskService.GetAll().Where(x => x.StatusId == id);

            return this.View(taskCategory);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return this.View(new TaskCategoryDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TaskCategoryDto category)
        {
            var result = await this.validator.ValidateAsync(category);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return this.View("Add", category);
            }

            var categoryId = await this.taskCategoryService.AddAsync(category);

            return this.RedirectToAction("Details", new { id = categoryId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var taskCategory = await this.taskCategoryService.GetByIdAsync(id);

            return this.View(taskCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskCategoryDto category)
        {
            var result = await this.validator.ValidateAsync(category);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return this.View("Edit", category);
            }

            var categoryId = await this.taskCategoryService.UpdateAsync(category.Id, category);

            return this.RedirectToAction("Details", new { id = categoryId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.taskCategoryService.DeleteAsync(id);

            return this.RedirectToAction("Index");
        }
    }
}
