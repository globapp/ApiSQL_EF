namespace Api.Models
{
    public class UserLog
    {
        public long Id { get; set; }
        public string? UserName { get; set; }
        public string? Action { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
