using Application.DTOs;
using Application.Interfaces.Services;
using Domain.Interfaces.Repositories.Base;
using Domain.Models;
using Infrastructure.Identity.Models;
using System.Linq.Expressions;

namespace Application.Services;

public class PostService(IRepository<Post> repository, IRepository<User> userRepository) : IPostService
{
    private readonly IRepository<Post> _repository = repository;
    private readonly IRepository<User> _userRepository = userRepository;

    public async Task<List<GetPostDTO>> GetAsync(string? search)
    {
        Expression<Func<Post, bool>> expression = p => true;
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower();
            expression = p => p.Title.ToLower().Contains(search) || p.Content.ToLower().Contains(search);
        }

        var posts = await _repository.GetAsync(expression);

        var uniqueUserId = posts.Select(p => p.UserId.ToString()).Distinct().ToList();
        var users = await _userRepository.GetAsync(u => uniqueUserId.Contains(u.Id));

        var getPostsDTO = posts.Select(p => MapToGetPostDTO(p, users.First(u => u.Id == p.UserId.ToString())));
        return getPostsDTO.ToList();
    }

    public async Task<GetPostDTO> GetByIdAsync(Guid id)
    {
        var post = await _repository.GetByIdAsync(id);
        if (post == null) return null;

        var user = await _userRepository.GetByIdAsync(post.UserId.ToString());

        var postDTO = MapToGetPostDTO(post, user!);
        return postDTO;
    }

    public async Task<GetPostDTO> Add(AddPostDTO dto, CancellationToken cancellationToken)
    {
        var isValid = dto.Validate().IsValid;
        if (!isValid) throw new ArgumentException("Post is invalid.");

        var duplicatedPosts = await _repository.GetAsync(p => p.UserId == dto.UserId && p.Title == dto.Title && p.Title == dto.Content);
        if (duplicatedPosts.Count > 0) throw new ArgumentException("Post was already created.");

        var post = new Post(dto.Title, dto.Content, dto.UserId);
        _repository.Add(post);

        if (!await _repository.CommitAsync(cancellationToken)) throw new ApplicationException("Was not possible to save the post.");

        var user = await _userRepository.GetByIdAsync(post.UserId.ToString());
        return MapToGetPostDTO(post, user!);
    }

    public async Task<GetPostDTO> Update(Guid id, AddPostDTO dto, CancellationToken cancellationToken)
    {
        var isValid = dto.Validate().IsValid;
        if (!isValid) throw new ArgumentException("Post is invalid.");

        var post = await _repository.GetByIdAsync(id) ?? throw new ArgumentException("Post does no exists.");
        if (post!.UserId != dto.UserId) throw new UnauthorizedAccessException();

        var duplicatedPosts = await _repository.GetAsync(p => p.UserId == dto.UserId && p.Title == dto.Title && p.Title == dto.Content);
        if (duplicatedPosts.Count > 0) throw new ArgumentException("Post was already exists.");

        post.Update(dto.Title, dto.Content);
        _repository.Update(post);

        if (!await _repository.CommitAsync(cancellationToken)) throw new ApplicationException("Was not possible to save the post.");

        var user = await _userRepository.GetByIdAsync(post.UserId.ToString());
        return MapToGetPostDTO(post, user!);
    }

    public async Task Delete(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        var post = await _repository.GetByIdAsync(id) ?? throw new ArgumentException("Post not found.");
        if (post.UserId != userId) throw new UnauthorizedAccessException();

        _repository.Delete(post);
        if (!await _repository.CommitAsync(cancellationToken)) throw new ApplicationException("Was not possible to save the post.");
    }

    private static GetPostDTO MapToGetPostDTO(Post post, User user) =>
        new(post.Id, post.Title, post.Content, user.UserName!, post.CreatedAt);
}
