using ToDoApplication.Domain.Models.Base;

namespace ToDoApplication.Domain.Models
{
    /// <summary>
    /// Class represents user task.
    /// </summary>
    public class ToDoTask : BaseEntity
    {
        /// <summary>
        /// Gets or sets task creation date.
        /// </summary>
        public DateTimeOffset CreationDate { get; set; }

        /// <summary>
        /// Gets or sets task due date.
        /// </summary>
        public DateTimeOffset DueDate { get; set; }

        /// <summary>
        /// Gets or sets task title.
        /// </summary>
        public string Tile { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets task description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether task reminder is active.
        /// </summary>
        public bool Reminder { get; set; }

        /// <summary>
        /// Gets or sets task reminder date.
        /// </summary>
        public DateTimeOffset ReminderDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether task is set as daily.
        /// </summary>
        public bool Daily { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether task is set as important.
        /// </summary>
        public bool Important { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether task is set as completed.
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Gets or sets associated ToDoList Id.
        /// </summary>
        public int ListId { get; set; }

        /// <summary>
        /// Gets or sets associated ToDoList.
        /// </summary>
        public virtual ToDoList List { get; set; } = new ToDoList();

        /// <summary>
        /// Gets or sets associated task status Id.
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// Gets or sets associated task status.
        /// </summary>
        public virtual TaskStatus Status { get; set; } = new TaskStatus();

        /// <summary>
        /// Gets or sets associated task category Id.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets associated task category.
        /// </summary>
        public virtual TaskCategory Category { get; set; } = new TaskCategory();

        /// <summary>
        /// Gets or sets associated task priority Id.
        /// </summary>
        public int PriorityId { get; set; }

        /// <summary>
        /// Gets or sets associated task priority.
        /// </summary>
        public virtual TaskPriority Priority { get; set; } = new TaskPriority();

        /// <summary>
        /// Gets or sets associated collection of task notes.
        /// </summary>
        public virtual ICollection<TaskNotes>? Notes { get; set; }
    }
}
