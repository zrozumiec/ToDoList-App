﻿using Microsoft.EntityFrameworkCore;
using ToDoApplication.Domain.Interfaces;
using ToDoApplication.Domain.Models;
using ToDoApplication.Infrastructure.Repositories.Base;

namespace ToDoApplication.Infrastructure.Repositories
{
    /// <summary>
    /// Class represents task category repository.
    /// </summary>
    public class TaskCategoryRepository : BaseRepository<TaskCategory>, ITaskCategoryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskCategoryRepository"/> class.
        /// </summary>
        /// <param name="dbContext">Application database context.</param>
        public TaskCategoryRepository(AppDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <inheritdoc/>
        protected override DbSet<TaskCategory> DbSet => this.DbContext.TaskCategories;

        /// <inheritdoc/>
        public override async Task<int> UpdateAsync(int id, TaskCategory newTaskCategory)
        {
            var taskCategoryInDatabase = await this.DbSet.FindAsync(id);

            taskCategoryInDatabase.Name = newTaskCategory.Name;
            taskCategoryInDatabase.Description = newTaskCategory.Description;

            await this.SaveAsync();

            return taskCategoryInDatabase.Id;
        }

        /// <inheritdoc/>
        public async Task<TaskCategory> GetByNameAsync(string name)
        {
            return await this.DbSet.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }

        /// <inheritdoc/>
        public async Task<TaskCategory> CheckIfExistInDataBaseWithSameNameAsync(string name)
        {
            return await this.DbSet.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }
    }
}
