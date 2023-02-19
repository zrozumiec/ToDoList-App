using Microsoft.Extensions.DependencyInjection;
using ToDoApplication.Domain.Interfaces;
using ToDoApplication.Infrastructure.Repositories;

namespace ToDoApplication.Infrastructure.DIServiceConfiguration
{
    /// <summary>
    /// Static class contains extensions method to configuration DI services.
    /// </summary>
    public static class IoCExtension
    {
        /// <summary>
        /// Inject application repositories to DI service.
        /// </summary>
        /// <param name="services">Extension IServiceCollection.</param>
        public static void RegisterRepository(this IServiceCollection services)
        {
            services.AddTransient<ITaskCategoryRepository, TaskCategoryRepository>();
            services.AddTransient<ITaskNotesRepository, TaskNotesRepository>();
            services.AddTransient<ITaskPriorityRepository, TaskPriorityRepository>();
            services.AddTransient<ITaskStatusesRepository, TaskStatusesRepository>();
            services.AddTransient<IToDoListRepository, ToDoListRepository>();
            services.AddTransient<IToDoTaskRepository, ToDoTaskRepository>();
        }
    }
}
