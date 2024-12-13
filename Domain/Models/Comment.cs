namespace Domain.Models;

public class Comment
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }

    public Comment(Guid id, string content, Guid userId)
    {
        Id = id;
        Content = content;
        CreatedAt = DateTime.Now;
        UserId = userId;
    }
}
