namespace ToDoApplication.Domain.Models.Base
{
    /// <summary>
    /// Base class for all models.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }
    }
}
