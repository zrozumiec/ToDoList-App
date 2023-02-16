using ToDoApplication.Domain.Models.Base;

namespace ToDoApplication.Domain.Models
{
    /// <summary>
    /// Class represents ToDoList.
    /// </summary>
    public class ToDoList : BaseEntity
    {
        /// <summary>
        /// Gets or sets list title.
        /// </summary>
        public string Tile { get; set; } = string.Empty;

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
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets associated user.
        /// </summary>
        public virtual ApplicationUser User { get; set; } = new ApplicationUser();

        /// <summary>
        /// Gets or sets collection of tasks connected to list.
        /// </summary>
        public virtual ICollection<Task>? Tasks { get; set; }
    }
}
