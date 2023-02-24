using System.Text.RegularExpressions;
using FluentValidation;
using ToDoApplication.Application.DTOs;

namespace ToDoApplication.Application.Validators
{
    /// <summary>
    /// Validator class for task category object.
    /// </summary>
    public class TaskCategoryValidator : AbstractValidator<TaskCategoryDto>
    {
        private readonly Regex regEx = new Regex("^[a-zA-Z]*$");

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskCategoryValidator"/> class.
        /// </summary>
        public TaskCategoryValidator()
        {
            this.RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100)
                .Matches(this.regEx).WithMessage("Category name can contains only characters.");

            this.RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .MaximumLength(250);
        }
    }
}
