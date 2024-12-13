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

    public Post(Guid id, string title, string content, Guid userId)
    {
        Id = id;
        CreatedAt = DateTime.Now;
        Title = title;
        Content = content;
        UserId = userId;
        _comments = [];
    }

    public Post(Guid id, string title, string content, Guid userId, IList<Comment> comments)
    {
        Id = id;
        CreatedAt = DateTime.Now;
        Title = title;
        Content = content;
        UserId = userId;

        _comments = [];
        _comments.AddRange(comments);
    }
}
