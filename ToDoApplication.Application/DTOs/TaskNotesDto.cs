﻿using ToDoApplication.Application.DTOs.Base;

namespace ToDoApplication.Application.DTOs
{
    /// <summary>
    /// Class represents task notes Dto.
    /// </summary>
    public class TaskNotesDto : BaseDto
    {
        /// <summary>
        /// Gets or sets associated task Id.
        /// </summary>
        public int TaskId { get; set; }
    }
}
