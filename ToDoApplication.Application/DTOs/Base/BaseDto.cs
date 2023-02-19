namespace ToDoApplication.Application.DTOs.Base
{
    /// <summary>
    /// Abstract class representing base Dto model.
    /// </summary>
    public abstract class BaseDto
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
