using ToDoApplication.Application.DTOs.Base;

namespace ToDoApplication.Application.DTOs
{
    /// <summary>
    /// Class represents task priority Dto.
    /// </summary>
    public class TaskPriorityDto : BaseDto
    {
        /// <summary>
        /// Gets or sets collections of associated task.
        /// </summary>
        public virtual IEnumerable<ToDoTaskDto> Tasks { get; set; } = new List<ToDoTaskDto>();
    }
}
