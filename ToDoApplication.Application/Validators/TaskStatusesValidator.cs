using System.Text.RegularExpressions;
using FluentValidation;
using ToDoApplication.Application.DTOs;

namespace ToDoApplication.Application.Validators
{
    /// <summary>
    /// Validator class for task statuses object.
    /// </summary>
    public class TaskStatusesValidator : AbstractValidator<TaskStatusesDto>
    {
        private readonly Regex regEx = new Regex(@"^[a-zA-Z\s]*$");

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskStatusesValidator"/> class.
        /// </summary>
        public TaskStatusesValidator()
        {
            this.RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100)
                .Matches(this.regEx).WithMessage("Priority name can contains only characters.");

            this.RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .MaximumLength(250);
        }
    }
}