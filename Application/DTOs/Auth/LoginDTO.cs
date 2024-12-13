namespace Application.DTOs.Auth;

public class LoginDTO(string email, string password)
{
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
}

public class TokenResponseDTO(string accessToken)
{
    public string AccessToken { get; set; } = accessToken;
}
