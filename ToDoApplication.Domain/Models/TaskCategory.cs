using ToDoApplication.Domain.Models.Base;

namespace ToDoApplication.Domain.Models
{
    /// <summary>
    /// Class represents task category.
    /// </summary>
    public class TaskCategory : BaseEntity
    {
        /// <summary>
        /// Gets or sets category name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets category name.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets collection of tasks with specify category.
        /// </summary>
        public virtual ICollection<ToDoTask>? Tasks { get; set; }
    }
}
