using System.Text.RegularExpressions;
using FluentValidation;
using ToDoApplication.Application.DTOs;

namespace ToDoApplication.Application.Validators
{
    /// <summary>
    /// Validator class for task priority object.
    /// </summary>
    public class TaskPriorityValidator : AbstractValidator<TaskPriorityDto>
    {
        private readonly Regex regEx = new Regex(@"^[a-zA-Z\s]*$");

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskPriorityValidator"/> class.
        /// </summary>
        public TaskPriorityValidator()
        {
            this.RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100)
                .Matches(this.regEx).WithMessage("Priority name can contains only characters.");
        }
    }
}
