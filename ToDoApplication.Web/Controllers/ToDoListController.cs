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
    public class ToDoListController : Controller
    {
        private readonly IToDoListService toDoListService;
        private readonly IToDoTaskService toDoTaskService;
        private readonly IValidator<ToDoListDto> validator;
        private readonly UserManager<ApplicationUser> userManager;

        public ToDoListController(
            IToDoListService toDoListService,
            IToDoTaskService toDoTaskService,
            IValidator<ToDoListDto> validator,
            UserManager<ApplicationUser> userManager)
        {
            this.toDoListService = toDoListService;
            this.toDoTaskService = toDoTaskService;
            this.validator = validator;
            this.userManager = userManager;
        }

        [Route("ToDoList/{showAll?}")]
        public IActionResult Index(bool showAll)
        {
            var lists = new ToDoListsVM();
            var userId = this.userManager.GetUserId(this.HttpContext.User);

            lists.ShowAll = showAll;

            lists.ToDoLists = this.toDoListService.GetAll().Where(x => x.UserId == userId);
            CountDefaultListItems.FillNumberOfTasks(userId, lists, this.toDoListService);

            if (!showAll)
            {
                lists.ToDoLists = lists.ToDoLists.Where(x => !x.IsHidden);
            }

            return this.View(lists);
        }

        [Route("ToDoList/Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var toDoTasks = this.toDoTaskService.GetAll(id);
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);
            var toDoList = await this.toDoListService.GetByIdAsync(id);

            toDoList.Tasks = toDoTasks;
            toDoList.User = user;

            return this.View(toDoList);
        }

        [HttpGet]
        [Route("ToDoList/Add")]
        public IActionResult Add()
        {
            return this.View(new ToDoListDto());
        }

        [Route("ToDoList/Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ToDoListDto toDoList)
        {
            toDoList.CreationDate = DateTimeOffset.Now;

            var result = await this.validator.ValidateAsync(toDoList);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return this.View("Add", toDoList);
            }

            string userId = this.userManager.GetUserId(this.HttpContext.User);

            toDoList.User = null!;
            toDoList.UserId = userId;

            var toDoListId = await this.toDoListService.AddAsync(toDoList);

            return this.RedirectToAction("Details", new { id = toDoListId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var toDoList = await this.toDoListService.GetByIdAsync(id);

            return this.View(toDoList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ToDoListDto toDoList)
        {
            var result = await this.validator.ValidateAsync(toDoList);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return this.View("Edit", toDoList);
            }

            var toDoListId = await this.toDoListService.UpdateAsync(toDoList.Id, toDoList);

            return this.RedirectToAction("Details", new { id = toDoListId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.toDoListService.DeleteAsync(id);

            return this.RedirectToAction("Index");
        }

        [Route("ToDoList/Copy/{listId:int}")]
        public async Task<IActionResult> Copy(int listId)
        {
            await this.toDoListService.Copy(listId);

            return this.RedirectToAction("Index");
        }
    }
}
