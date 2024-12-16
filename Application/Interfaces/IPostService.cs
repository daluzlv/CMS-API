using Application.DTOs;
using Domain.Models;
using System.Linq.Expressions;

namespace Application.Interfaces.Services;

public interface IPostService
{
    Task<List<GetPostDTO>> GetAsync(Expression<Func<Post, bool>>? expression);
    Task<GetPostDTO> GetByIdAsync(Guid id);
    Task<GetPostDTO> Add(AddPostDTO dto);
    void Delete(Guid id);
}
