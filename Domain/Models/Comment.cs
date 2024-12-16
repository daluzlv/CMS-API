namespace Domain.Models;

public class Comment
{
    public Guid Id { get; set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }

    public Comment(string content, Guid userId)
    {
        Content = content;
        CreatedAt = DateTime.Now;
        UserId = userId;
    }

    public void Update(string content) => Content = content;
}
