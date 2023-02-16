using Microsoft.AspNetCore.Identity;

namespace ToDoApplication.Domain.Models
{
    /// <summary>
    /// Extended user identity class.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets user ToDoLists.
        /// </summary>
        public virtual ICollection<ToDoList>? ToDoLists { get; set; }
    }
}
