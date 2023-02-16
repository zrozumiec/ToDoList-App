using ToDoApplication.Domain.Models.Base;

namespace ToDoApplication.Domain.Models
{
    /// <summary>
    /// Class represents task status.
    /// </summary>
    public class TaskStatus : BaseEntity
    {
        /// <summary>
        /// Gets or sets task status name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets task status description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets collections of associated task.
        /// </summary>
        public virtual ICollection<ToDoTask>? Tasks { get; set; }
    }
}
