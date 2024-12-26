using Domain.Interfaces.Repositories.Base;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Web;

namespace Infrastructure.Identity.UserManager;

public class CustomUserManager : UserManager<User>
{
    private readonly IRepository<User> _repository;
    private readonly IEmailSender _emailSender;

    public CustomUserManager(
        IUserStore<User> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<User> passwordHasher,
        IEnumerable<IUserValidator<User>> userValidators,
        IEnumerable<IPasswordValidator<User>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<User>> logger,
        IEmailSender emailSender,
        IRepository<User> repository) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        _repository = repository;
        _emailSender = emailSender;
    }

    public override async Task<IdentityResult> CreateAsync(User user, string password)
    {
        user.UserName = user.Email;
        var result = await base.CreateAsync(user, password);

        if (result.Succeeded)
        {
            var createdUser = await _repository.GetAsync(u => u.Email == user.Email).FirstAsync();

            var token = GenerateEmailConfirmationTokenAsync(createdUser).Result;

            await SendConfimationEmail(createdUser, HttpUtility.HtmlDecode(token));
        }

        return result;
    }

    private async Task SendConfimationEmail(User user, string token)
    {
        var url = @"http://localhost:4200/confirm-email?";
        var codedToken = Uri.EscapeDataString(token);
        url += $"userId={user.Id}&token={codedToken}";

        var body = $"<p>Confirme seu e-mail clicando <a href=\"{url}\">aqui</a>.</p>";

        await _emailSender.SendEmailAsync(user.Email!, "CMS - Confirme o seu e-mail", body);
    }
}
