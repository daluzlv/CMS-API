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

    public async Task<GetPostDTO> Add(AddPostDTO dto)
    {
        var isValid = dto.Validate().IsValid;
        if (!isValid) throw new ArgumentException("Post is invalid.");

        var duplicatedPosts = await _repository.GetAsync(p => p.UserId == dto.UserId && p.Title == dto.Title && p.Title == dto.Content);
        if (duplicatedPosts.Count > 0) throw new ArgumentException("Post was already created");

        var post = new Post(dto.Title, dto.Content, dto.UserId);
        _repository.Add(post);

        if (!await _repository.CommitAsync()) throw new ApplicationException("Was not possible to save the post.");

        var user = await _userRepository.GetByIdAsync(post.UserId.ToString());
        return MapToGetPostDTO(post, user);
    }

    public void Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<GetPostDTO>> GetAsync(Expression<Func<Post, bool>>? expression)
    {
        throw new NotImplementedException();
    }

    public async Task<GetPostDTO> GetByIdAsync(Guid id)
    {
        var post = await _repository.GetByIdAsync(id);
        if (post == null) return null;

        var user = await _userRepository.GetByIdAsync(post.UserId);

        var postDTO = MapToGetPostDTO(post, user!);
        return postDTO;
    }

    private static GetPostDTO MapToGetPostDTO(Post post, User user)
    {
        var userDTO = new UserDTO(Guid.Parse(user.Id), user.UserName!, user.Email!);

        return new GetPostDTO(post.Id, post.Title, post.Content, post.CreatedAt, userDTO);
    }
}
