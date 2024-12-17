using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Validator;

namespace Application.DTOs;

public class UserDTO(Guid id, string fullName, string email)
{
    public Guid Id { get; private set; } = id;
    public string FullName { get; private set; } = fullName;
    public string Email { get; private set; } = email;
}

public class ApiUpdateUserDTO(string fullName)
{
    public string Fullname { get; private set; } = fullName;
}

public class UpdateUserDTO(Guid id, string fullName)
{
    public Guid Id { get; private set; } = id;
    public string Fullname { get; private set; } = fullName;

    public ValidationResult Validate()
    {
        var validator = new Validator<UpdateUserDTO>();

        validator.RuleFor(u => u.Id)
            .NotEmpty()
            .WithMessage("Id cannot be empty")
            .NotNull()
            .WithMessage("Id cannot be empty");

        validator.RuleFor(u => u.Fullname)
            .NotEmpty()
            .WithMessage("Full name cannot be empty")
            .NotNull()
            .WithMessage("Full name cannot be empty")
            .MinimumLength(3)
            .WithMessage("Name is invalid");

        return validator.Validate(this);
    }
}
