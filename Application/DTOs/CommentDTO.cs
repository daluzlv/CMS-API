using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Validator;

namespace Application.DTOs;

public class GetCommentDTO(Guid id, string content, DateTime createdAt, string username)
{
    public Guid Id { get; set; } = id;
    public string Content { get; set; } = content;
    public DateTime CreatedAt { get; set; } = createdAt;
    public string Username { get; set; } = username;
}

public class ApiAddCommentDTO(string content)
{
    public string Content { get; set; } = content;
}

public class AddCommentDTO(string content, Guid userId)
{
    public string Content { get; set; } = content;
    public Guid UserId { get; set; } = userId;

    public ValidationResult Validate()
    {
        var validator = new Validator<AddCommentDTO>();

        validator.RuleFor(c => c.Content)
            .NotEmpty()
            .WithMessage("Content cannot be empty")
            .NotNull()
            .WithMessage("Content cannot be empty");

        validator.RuleFor(c => c.Content)
            .NotEmpty()
            .WithMessage("User is required")
            .NotNull()
            .WithMessage("User is required");

        return validator.Validate(this);
    }
}
