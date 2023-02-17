using ToDoApplication.Application.DTOs.Base;

namespace ToDoApplication.Application.DTOs
{
    /// <summary>
    /// Class represents task category Dto.
    /// </summary>
    public class TaskCategoryDto : BaseDto
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
        public virtual ICollection<ToDoTaskDto> Tasks { get; set; } = new List<ToDoTaskDto>();
    }
}
