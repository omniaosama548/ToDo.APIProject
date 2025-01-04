using System.ComponentModel.DataAnnotations;
using ToDo.API.Models;

namespace ToDo.API.DTOs
{
    public class UpdatedTaskDTO
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;
        [Required]
        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public TasksStatus Status { get; set; }
    }
}
