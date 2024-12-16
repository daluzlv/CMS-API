using Api.Controllers.Base;
using Application.DTOs;
using Application.Interfaces.Services;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("post")]
public class PostController(IPostService service, UserManager<User> userManager) : BaseController(userManager)
{
    private readonly IPostService _service = service;

    [HttpPost, Authorize]
    public async Task<IActionResult> Post([FromBody] ApiAddPostDTO apiAddPostDTO)
    {
        try
        {
            var userId = await GetUserIdAsync();
            if (userId == null) return Unauthorized();

            var addPostDTO = new AddPostDTO(apiAddPostDTO.Title, apiAddPostDTO.Content, userId.Value);
            var post = await _service.Add(addPostDTO);

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
