using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Models;

public class AuthResult
{
    private readonly List<IdentityError> _errors = new List<IdentityError>();
    public bool Succeeded { get; set; }
    public AuthResult(bool sucess)
    {
        Succeeded = sucess;
    }
    public AuthResult(bool sucess, IEnumerable<IdentityError> errors)
    {
        Succeeded = sucess;
        _errors.AddRange(errors);
    }
    public AuthResult(bool sucess, IdentityError error)
    {
        Succeeded = sucess;
        _errors.Add(error);
    }

    public IEnumerable<IdentityError> Errors => _errors;

    public void AddError(IdentityError error)
    {
        _errors.Add(error);
    }
    public void AddErrors(IEnumerable<IdentityError> errors)
    {
        _errors.AddRange(errors);
    }
}
