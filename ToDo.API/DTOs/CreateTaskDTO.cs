using System.ComponentModel.DataAnnotations;

namespace ToDo.API.DTOs
{
    public class CreateTaskDTO
    {
       [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;
        [Required]
        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }

    }
}
