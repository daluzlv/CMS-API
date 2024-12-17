using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces.Repositories.Base;
using Infrastructure.Identity.Models;

namespace Application.Services;

public class UserService(IRepository<User> repository) : IUserService
{
    private readonly IRepository<User> _repository = repository;

    public async Task<UserDTO> Update(UpdateUserDTO dto, Guid userId, CancellationToken cancellationToken)
    {
        var validator = dto.Validate();
        if (!validator.IsValid) throw new ArgumentException(string.Join(",", validator.Errors.Select(e => e.ErrorMessage)));

        if (dto.Id != userId) throw new UnauthorizedAccessException();

        var user = await _repository.GetByIdAsync(dto.Id.ToString());
        if (user == null) throw new ArgumentException("User not found");

        user.UpdateFullName(dto.Fullname);
        _repository.Update(user);

        if (!await _repository.CommitAsync(cancellationToken)) throw new ApplicationException("Was not possible to save the user");

        return new UserDTO(Guid.Parse(user.Id), user.FullName, user.Email!);
    }
}
