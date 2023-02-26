using ToDoApplication.Application.DTOs.Base;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Application.DTOs
{
    /// <summary>
    /// Class represents ToDoList Dto.
    /// </summary>
    public class ToDoListDto : BaseDto
    {
        /// <summary>
        /// Gets or sets list description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether list is hidden.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets list creation date.
        /// </summary>
        public DateTimeOffset CreationDate { get; set; }

        /// <summary>
        /// Gets or sets associated user id.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets associated user.
        /// </summary>
        public virtual ApplicationUser User { get; set; } = new ApplicationUser();

        /// <summary>
        /// Gets or sets number of tasks in list.
        /// </summary>
        public int NumberOfTasks { get; set; }

        /// <summary>
        /// Gets or sets collection of tasks connected to list.
        /// </summary>
        public virtual IEnumerable<ToDoTaskDto> Tasks { get; set; } = new List<ToDoTaskDto>();
    }
}
