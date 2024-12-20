using Api.Controllers.Base;
using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("user")]
public class UserController(IUserService service, UserManager<User> userManager) : BaseController(userManager)
{
    private readonly IUserService _service = service;

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> Get(Guid id)
    {
        try
        {
            var user = await _service.GetByIdAsync(id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id}"), Authorize]
    public async Task<ActionResult<UserDTO>> Put(Guid id, [FromBody] ApiUpdateUserDTO dto, CancellationToken cancellationToken)
    {
        try
        {
            var userId = await GetUserIdAsync();
            if (userId == null) return Unauthorized();

            var updateDTO = new UpdateUserDTO(id, dto.Fullname);


            var user = await _service.Update(updateDTO, userId.Value, cancellationToken);

            return Ok(user);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
