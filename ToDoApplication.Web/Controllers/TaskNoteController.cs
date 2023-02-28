using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces;
using ToDoApplication.Web.Models.ViewModels;

namespace ToDoApplication.Web.Controllers
{
    [Authorize]
    public class TaskNoteController : Controller
    {
        private readonly ITaskNotesService taskNotesService;
        private readonly IValidator<TaskNotesDto> validator;

        public TaskNoteController(
            ITaskNotesService taskNotesService,
            IValidator<TaskNotesDto> validator)
        {
            this.taskNotesService = taskNotesService;
            this.validator = validator;
        }

        [Route("ToDoList/Task/{taskId:int}/TaskNotes")]
        public IActionResult Index(int taskId, bool showAll)
        {
            var notes = this.taskNotesService.GetAll(taskId);

            var listWithNotes = new TaskWithNotesVM
            {
                TaskId = taskId,
                ShowAll = showAll,
                TaskNotes = notes,
            };

            return this.View(listWithNotes);
        }

        [Route("ToDoList/TaskNote/Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var note = new TaskNoteDetailVM
            {
                TaskNote = await this.taskNotesService.GetByIdAsync(id)
            };

            return this.View(note);
        }

        [Route("ToDoList/TaskNote/{taskId:int}/Add")]
        [HttpGet]
        public IActionResult Add(int taskId)
        {
            TaskNotesDto taskNoteDto = new ()
            {
                TaskId = taskId
            };
            return this.View(taskNoteDto);
        }

        [Route("ToDoList/TaskNote/{taskId:int}/Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TaskNotesDto taskNote)
        {
            var result = await this.validator.ValidateAsync(taskNote);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return this.View("Add", taskNote);
            }

            var taskNoteId = await this.taskNotesService.AddAsync(taskNote);

            return this.RedirectToAction("Details", new { id = taskNoteId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var taskNote = await this.taskNotesService.GetByIdAsync(id);

            return this.View(taskNote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskNotesDto taskNote)
        {
            var result = await this.validator.ValidateAsync(taskNote);

            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);

                return this.View("Edit", taskNote);
            }

            var taskNoteId = await this.taskNotesService.UpdateAsync(taskNote.Id, taskNote);

            return this.RedirectToAction("Details", new { id = taskNoteId });
        }

        public async Task<IActionResult> Delete(int id, int taskId)
        {
            await this.taskNotesService.DeleteAsync(id);

            return this.RedirectToAction("Index", "TaskNote", new { taskId });
        }
    }
}