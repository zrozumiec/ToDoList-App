using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ToDoApplication.Application.DTOs;
using ToDoApplication.Application.Interfaces;
using ToDoApplication.Application.Mapper;
using ToDoApplication.Application.Services;
using ToDoApplication.Application.Validators;

namespace ToDoApplication.Application.DIServiceConfiguration
{
    /// <summary>
    /// Static class contains extensions method to configuration DI services.
    /// </summary>
    public static class IoCExtension
    {
        /// <summary>
        /// Inject application services to DI service.
        /// </summary>
        /// <param name="services">Extension IServiceCollection.</param>
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<ITaskCategoryService, TaskCategoryService>();
            services.AddTransient<ITaskNotesService, TaskNotesService>();
            services.AddTransient<ITaskPriorityService, TaskPriorityService>();
            services.AddTransient<ITaskStatusesService, TaskStatusesService>();
            services.AddTransient<IToDoTaskService, ToDoTaskService>();
            services.AddTransient<IToDoListService, ToDoListService>();
        }

        /// <summary>
        /// Inject AutoMapper profiles to DI service.
        /// </summary>
        /// <param name="services">Extension IServiceCollection.</param>
        public static void RegisterMappers(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(TaskCategoryMappingProfile));
            services.AddAutoMapper(typeof(TaskNotesMappingProfile));
            services.AddAutoMapper(typeof(TaskPriorityMappingProfile));
            services.AddAutoMapper(typeof(TaskStatusesMappingProfile));
            services.AddAutoMapper(typeof(ToDoTaskMappingProfile));
            services.AddAutoMapper(typeof(ToDoListMappingProfile));
        }

        /// <summary>
        /// Inject application validators to DI service.
        /// </summary>
        /// <param name="services">Extension IServiceCollection.</param>
        public static void RegisterValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<TaskCategoryDto>, TaskCategoryValidator>();
            services.AddScoped<IValidator<TaskNotesDto>, TaskNotesValidator>();
            services.AddScoped<IValidator<TaskPriorityDto>, TaskPriorityValidator>();
            services.AddScoped<IValidator<TaskStatusesDto>, TaskStatusesValidator>();
            services.AddScoped<IValidator<ToDoListDto>, ToDoListValidator>();
            services.AddScoped<IValidator<ToDoTaskDto>, ToDoTaskValidator>();
        }
    }
}
