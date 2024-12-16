using Application.DTOs;

namespace Application.Interfaces;

public interface ICommentService
{
    Task<GetCommentDTO> GetByIdAsync(Guid id);
    Task<GetCommentDTO> Update(Guid id, AddCommentDTO dto, CancellationToken cancellationToken);
    Task Delete(Guid id, Guid userId, CancellationToken cancellationToken);
}
