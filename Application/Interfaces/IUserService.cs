using Application.DTOs;

namespace Application.Interfaces;

public interface IUserService
{
    Task<UserDTO> Update(UpdateUserDTO dto, Guid userId, CancellationToken cancellationToken);
}
