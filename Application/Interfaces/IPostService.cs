using Application.DTOs;

namespace Application.Interfaces.Services;

public interface IPostService
{
    Task<List<GetPostDTO>> GetAsync(string? search);
    Task<GetPostDTO> GetByIdAsync(Guid id);
    Task<GetPostDTO> Add(AddPostDTO dto, CancellationToken cancellationToken);
    Task<GetPostDTO> Update(Guid id, AddPostDTO dto, CancellationToken cancellationToken);
    Task Delete(Guid id, Guid userId, CancellationToken cancellationToken);
}
