using System.Text.RegularExpressions;
using FluentValidation;
using ToDoApplication.Application.DTOs;

namespace ToDoApplication.Application.Validators
{
    /// <summary>
    /// Validator class for ToDoList object.
    /// </summary>
    public class ToDoListValidator : AbstractValidator<ToDoListDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoListValidator"/> class.
        /// </summary>
        public ToDoListValidator()
        {
            this.RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100);

            this.RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .MaximumLength(250);
        }
    }
}