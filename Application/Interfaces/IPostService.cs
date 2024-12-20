using Application.DTOs;

namespace Application.Interfaces;

public interface IPostService
{
    Task<List<GetPostByIdDTO>> GetAsync(string? search);
    Task<GetPostByIdDTO> GetByIdAsync(Guid id);
    Task<GetPostDTO> Add(AddPostDTO dto, CancellationToken cancellationToken);
    Task<GetPostDTO> Update(Guid id, AddPostDTO dto, CancellationToken cancellationToken);
    Task Delete(Guid id, Guid userId, CancellationToken cancellationToken);
    Task<GetCommentDTO> Comment(Guid id, AddCommentDTO dto, CancellationToken cancellationToken);
}
