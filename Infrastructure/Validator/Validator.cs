using FluentValidation;

namespace Infrastructure.Validator;

public class Validator<T> : AbstractValidator<T> where T : class { }
