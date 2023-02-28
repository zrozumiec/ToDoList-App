namespace ToDoApplication.Web.Models.ViewModels
{
    public class ReminderVM
    {
        public List<int> TaskId { get; set; } = new List<int>();

        public List<int> ListId { get; set; } = new List<int>();

        public List<string> Name { get; set; } = new List<string>();
    }
}
