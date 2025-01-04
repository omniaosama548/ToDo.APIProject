using ToDo.API.Models;

namespace ToDo.API.DTOs
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }

        public string UserId { get; set; }
        public string User { get; set; }
        public TasksStatus Status { get; set; } = TasksStatus.New;
    }
}
