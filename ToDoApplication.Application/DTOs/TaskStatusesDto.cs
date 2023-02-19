﻿using ToDoApplication.Application.DTOs.Base;

namespace ToDoApplication.Application.DTOs
{
    /// <summary>
    /// Class represents task status Dto.
    /// </summary>
    public class TaskStatusesDto : BaseDto
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
        public virtual ICollection<ToDoTaskDto> Tasks { get; set; } = new List<ToDoTaskDto>();
    }
}