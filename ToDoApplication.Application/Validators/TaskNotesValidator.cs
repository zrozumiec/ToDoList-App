using FluentValidation;
using ToDoApplication.Application.DTOs;

namespace ToDoApplication.Application.Validators
{
    /// <summary>
    /// Validator class for task notes object.
    /// </summary>
    public class TaskNotesValidator : AbstractValidator<TaskNotesDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskNotesValidator"/> class.
        /// </summary>
        public TaskNotesValidator()
        {
            this.RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(250);
        }
    }
}
