using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Validator;

namespace Application.DTOs;

public class ApiAddPostDTO(string title, string content, string bannerUrl)
{
    public string Title { get; set; } = title;
    public string Content { get; set; } = content;
    public string BannerUrl { get; set; } = bannerUrl;
}

public class GetPostDTO(Guid id, string title, string content, string bannerUrl, string fullName, DateTime createdAt)
{
    public Guid Id { get; set; } = id;
    public string Title { get; set; } = title;
    public string Content { get; set; } = content;
    public string BannerUrl { get; set; } = bannerUrl;
    public string FullName { get; set; } = fullName;
    public DateTime CreatedAt { get; set; } = createdAt;
}

public class AddPostDTO(string title, string content, string bannerUrl, Guid userId)
{
    public string Title { get; set; } = title;
    public string Content { get; set; } = content;
    public string BannerUrl { get; set; } = bannerUrl;
    public Guid UserId { get; set; } = userId;

    public ValidationResult Validate()
    {
        var validator = new Validator<AddPostDTO>();

        validator.RuleFor(p => p.Title)
            .NotEmpty()
            .WithMessage("Title cannot be empty")
            .NotNull()
            .WithMessage("Title cannot be empty");

        validator.RuleFor(p => p.Content)
            .NotEmpty()
            .WithMessage("Content cannot be empty")
            .NotNull()
            .WithMessage("Content cannot be empty");

        validator.RuleFor(p => p.UserId)
            .NotEmpty()
            .WithMessage("UserId cannot be empty")
            .NotNull()
            .WithMessage("UserId cannot be empty");

        return validator.Validate(this);
    }
}
