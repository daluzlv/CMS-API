namespace Domain.Models;

public class Post
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid UserId { get; set; }

    private readonly List<Comment> _comments;
    public IEnumerable<Comment> Comments => _comments;

    public Post(string title, string content, Guid userId)
    {
        CreatedAt = DateTime.Now;
        Title = title;
        Content = content;
        UserId = userId;
        _comments = [];
    }

    public Post(string title, string content, Guid userId, IList<Comment> comments) : this(title, content, userId)
    {
        _comments.AddRange(comments);
    }

    public void Update(string title, string content)
    {
        Title = title;
        Content = content;
    }
}
