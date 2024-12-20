using Microsoft.EntityFrameworkCore;

using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces.Repositories.Base;
using Domain.Models;
using Infrastructure.Identity.Models;

namespace Application.Services;

public class CommentService(IRepository<Comment> repository, IRepository<User> userRepository) : ICommentService
{
    private readonly IRepository<Comment> _repository = repository;
    private readonly IRepository<User> _userRepository = userRepository;

    public async Task<GetCommentDTO> GetByIdAsync(Guid id)
    {
        var comment = await _repository.GetByIdAsync(id);
        if (comment == null) return null;

        var user = await _userRepository.GetByIdAsync(comment.UserId.ToString());

        var dto = MapToGetCommentDTO(comment, user!);
        return dto;
    }

    public async Task Delete(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        var comment = await _repository.GetByIdAsync(id) ?? throw new ArgumentException("Comment not found");
        if (comment.UserId != userId) throw new UnauthorizedAccessException();

        _repository.Delete(comment);
        if (!await _repository.CommitAsync(cancellationToken)) throw new ApplicationException("Was not possible to delete the comment");
    }

    public async Task<GetCommentDTO> Update(Guid id, AddCommentDTO dto, CancellationToken cancellationToken)
    {
        var validator = dto.Validate();
        if (!validator.IsValid) throw new ArgumentException(string.Join(",", validator.Errors.Select(e => e.ErrorMessage)));

        var comment = await _repository.GetByIdAsync(id) ?? throw new ArgumentException("Comment not found");
        if (comment!.UserId != dto.UserId) throw new UnauthorizedAccessException();

        comment.Update(dto.Content);
        _repository.Update(comment);

        if (!await _repository.CommitAsync(cancellationToken)) throw new ApplicationException("Was not possible to save the comment");

        var user = await _userRepository.GetByIdAsync(comment.UserId.ToString());
        return MapToGetCommentDTO(comment, user!);
    }

    private static GetCommentDTO MapToGetCommentDTO(Comment comment, User user) =>
        new(comment.Id, comment.Content, comment.CreatedAt, user.FullName!);
}
