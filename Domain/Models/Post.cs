namespace Domain.Models;

public class Post
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Title { get; private set; }
    public string Content { get; private set; }
    public string BannerUrl { get; private set; }
    public Guid UserId { get; set; }

    private readonly List<Comment> _comments;
    public IEnumerable<Comment> Comments => _comments;

    public Post(string title, string content, string bannerUrl, Guid userId)
    {
        CreatedAt = DateTime.Now;
        Title = title;
        Content = content;
        UserId = userId;
        BannerUrl = bannerUrl;
        _comments = [];
    }

    public Post(string title, string content, string bannerUrl, Guid userId, IList<Comment> comments) : this(title, content, bannerUrl, userId)
    {
        _comments.AddRange(comments);
    }

    public void Update(string title, string content, string bannerUrl)
    {
        Title = title;
        Content = content;
        BannerUrl = bannerUrl;
    }

    public void AddComment(Comment comment) => _comments.Add(comment);
}
