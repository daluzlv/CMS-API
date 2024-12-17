using Api.Controllers.Base;
using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("comment")]
public class CommentController(ICommentService service, UserManager<User> userManager) : BaseController(userManager)
{
    private readonly ICommentService _service = service;

    [HttpGet("{id}")]
    public async Task<ActionResult<GetCommentDTO>> GetById(Guid id)
    {
        try
        {
            var comment = await _service.GetByIdAsync(id);
            return Ok(comment);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id}"), Authorize]
    public async Task<ActionResult<GetCommentDTO>> Put(Guid id, [FromBody] ApiAddCommentDTO dto, CancellationToken cancellationToken)
    {
        try
        {
            var userId = await GetUserIdAsync();
            if (userId == null) return Unauthorized();

            var addCommentDTO = new AddCommentDTO(dto.Content, userId.Value);
            var comment = await _service.Update(id, addCommentDTO, cancellationToken);

            return Ok(comment);
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

    [HttpDelete, Authorize]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var userId = await GetUserIdAsync();
            if (userId == null) return Unauthorized();

            await _service.Delete(id, userId.Value, cancellationToken);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
