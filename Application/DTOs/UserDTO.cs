namespace Application.DTOs;

public class UserDTO(Guid id, string username, string email)
{
    public Guid Id { get; set; } = id;
    public string Username { get; set; } = username;
    public string Email { get; set; } = email;
}
