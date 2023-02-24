using System.Text.RegularExpressions;
using FluentValidation;
using ToDoApplication.Application.DTOs;

namespace ToDoApplication.Application.Validators
{
    /// <summary>
    /// Validator class for task object.
    /// </summary>
    public class ToDoTaskValidator : AbstractValidator<ToDoTaskDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoTaskValidator"/> class.
        /// </summary>
        public ToDoTaskValidator()
        {
            this.RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(250);

            this.RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .MaximumLength(250);

            this.RuleFor(x => x.DueDate)
                .GreaterThan(x => x.CreationDate);

            this.RuleFor(x => x.ReminderDate)
                .GreaterThan(x => x.CreationDate);
        }
    }
}