namespace ToDo.API.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<DoTask> Tasks { get; set; } = new List<DoTask>();
    }
}
