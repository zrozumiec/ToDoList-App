using ToDoApplication.Application.DTOs.Base;

namespace ToDoApplication.Application.DTOs
{
    /// <summary>
    /// Class represents task priority Dto.
    /// </summary>
    public class TaskPriorityDto : BaseDto
    {
        /// <summary>
        /// Gets or sets priority name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets collections of associated task.
        /// </summary>
        public virtual ICollection<ToDoTaskDto> Tasks { get; set; } = new List<ToDoTaskDto>();
    }
}
