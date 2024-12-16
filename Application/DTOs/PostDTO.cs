using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Validator;

namespace Application.DTOs;

public class ApiAddPostDTO(string title, string content)
{
    public string Title { get; set; } = title;
    public string Content { get; set; } = content;
}

public class GetPostDTO(Guid id, string title, string content, DateTime createdAt, UserDTO user)
{
    public Guid Id { get; set; } = id;
    public string Title { get; set; } = title;
    public string Content { get; set; } = content;
    public DateTime CreatedAt { get; set; } = createdAt;
    public UserDTO User { get; set; } = user;
}

public class AddPostDTO(string title, string content, Guid userId)
{
    public string Title { get; set; } = title;
    public string Content { get; set; } = content;
    public Guid UserId { get; set; } = userId;

    public ValidationResult Validate()
    {
        var validator = new Validator<AddPostDTO>();

        validator.RuleFor(p => p.Title)
            .NotEmpty()
            .WithMessage("Título não pode ser vazio")
            .NotNull()
            .WithMessage("Título não pode ser vazio");

        validator.RuleFor(p => p.Content)
            .NotEmpty()
            .WithMessage("Título não pode ser vazio")
            .NotNull()
            .WithMessage("Título não pode ser vazio");

        validator.RuleFor(p => p.UserId)
            .NotEmpty()
            .WithMessage("Título não pode ser vazio")
            .NotNull()
            .WithMessage("Título não pode ser vazio");

        return validator.Validate(this);
    }
}
