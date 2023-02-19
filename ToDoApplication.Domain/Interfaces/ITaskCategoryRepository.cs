﻿using ToDoApplication.Domain.Interfaces.Base;
using ToDoApplication.Domain.Models;

namespace ToDoApplication.Domain.Interfaces
{
    /// <summary>
    /// Interface for task category repository inherit from base interface repository.
    /// </summary>
    public interface ITaskCategoryRepository : IBaseRepository<TaskCategory>
    {
        /// <summary>
        /// Update existing task category in database.
        /// </summary>
        /// <param name="id">Id of task category to update.</param>
        /// <param name="newTaskCategory">New task category data.</param>
        /// <returns>Updated task category id.</returns>
        new Task<int> UpdateAsync(int id, TaskCategory newTaskCategory);
    }
}