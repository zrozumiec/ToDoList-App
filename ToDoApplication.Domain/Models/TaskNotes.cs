using ToDoApplication.Domain.Models.Base;

namespace ToDoApplication.Domain.Models
{
    /// <summary>
    /// Class represents task notes.
    /// </summary>
    public class TaskNotes : BaseEntity
    {
        /// <summary>
        /// Gets or sets task note description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets associated task Id.
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// Gets or sets associated task.
        /// </summary>
        public virtual Task ToDoTask { get; set; } = new Task();
    }
}
