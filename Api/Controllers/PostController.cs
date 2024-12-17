using Api.Controllers.Base;
using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("post")]
public class PostController(IPostService service, UserManager<User> userManager) : BaseController(userManager)
{
    private readonly IPostService _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetPostDTO>>> Get(string? search)
    {
        try
        {
            var posts = await _service.GetAsync(search);
            return Ok(posts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetPostDTO>> GetById(Guid id)
    {
        try
        {
            var post = await _service.GetByIdAsync(id);
            return Ok(post);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost, Authorize]
    public async Task<ActionResult<GetPostDTO>> Post([FromBody] ApiAddPostDTO dto, CancellationToken cancellationToken)
    {
        try
        {
            var userId = await GetUserIdAsync();
            if (userId == null) return Unauthorized();

            var addPostDTO = new AddPostDTO(dto.Title, dto.Content, userId.Value);
            var post = await _service.Add(addPostDTO, cancellationToken);

            return Ok(post);
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

    [HttpPut("{id}"), Authorize]
    public async Task<ActionResult<GetPostDTO>> Put(Guid id, [FromBody] ApiAddPostDTO dto, CancellationToken cancellationToken)
    {
        try
        {
            var userId = await GetUserIdAsync();
            if (userId == null) return Unauthorized();

            var addPostDTO = new AddPostDTO(dto.Title, dto.Content, userId.Value);
            var post = await _service.Update(id, addPostDTO, cancellationToken);

            return Ok(post);
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

    [HttpPost("{id}/comment"), Authorize]
    public async Task<ActionResult<GetCommentDTO>> Comment(Guid id, [FromBody] ApiAddCommentDTO dto, CancellationToken cancellationToken)
    {
        try
        {
            var userId = await GetUserIdAsync();
            if (userId == null) return Unauthorized();

            var addCommentDTO = new AddCommentDTO(dto.Content, userId.Value);
            var post = await _service.Comment(id, addCommentDTO, cancellationToken);

            return Ok(post);
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
