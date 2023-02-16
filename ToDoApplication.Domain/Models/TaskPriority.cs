using ToDoApplication.Domain.Models.Base;

namespace ToDoApplication.Domain.Models
{
    /// <summary>
    /// Class represents task priority.
    /// </summary>
    public class TaskPriority : BaseEntity
    {
        /// <summary>
        /// Gets or sets priority name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets collections of associated task.
        /// </summary>
        public virtual ICollection<ToDoTask>? Tasks { get; set; }
    }
}
