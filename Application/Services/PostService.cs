﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces.Repositories.Base;
using Domain.Models;
using Infrastructure.Identity.Models;

namespace Application.Services;

public class PostService(IRepository<Post> repository, IRepository<User> userRepository) : IPostService
{
    private readonly IRepository<Post> _repository = repository;
    private readonly IRepository<User> _userRepository = userRepository;

    public async Task<List<GetPostByIdDTO>> GetAsync(string? search)
    {
        Expression<Func<Post, bool>> expression = p => true;
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower();
            expression = p => p.Title.ToLower().Contains(search) || p.Content.ToLower().Contains(search);
        }

        var posts = await _repository.GetAsync(expression).OrderByDescending(p => p.CreatedAt).ToListAsync();

        var uniqueUserId = posts.Select(p => p.UserId.ToString()).Distinct().ToList();
        var users = await _userRepository.GetAsync(u => uniqueUserId.Contains(u.Id)).ToListAsync();

        var dto = posts.Select(p => MapToGetPostByIdDTO(p, users.First(u => u.Id == p.UserId.ToString())));
        return dto.ToList();
    }

    public async Task<GetPostByIdDTO> GetByIdAsync(Guid id)
    {
        var post = await _repository.GetAsync(p => p.Id == id).Include(p => p.Comments).FirstOrDefaultAsync();
        if (post == null) return null;

        var user = await _userRepository.GetByIdAsync(post.UserId.ToString());

        var dto = MapToGetPostByIdDTO(post, user!);

        var uniqueUserId = post.Comments.Select(p => p.UserId.ToString()).Distinct().ToList();
        var users = await _userRepository.GetAsync(u => uniqueUserId.Contains(u.Id)).ToListAsync();

        dto.Comments = post.Comments.Select(c => MapToGetCommentDTO(c, users.First(u => u.Id == c.UserId.ToString()))).ToList();

        return dto;
    }

    public async Task<GetPostDTO> Add(AddPostDTO dto, CancellationToken cancellationToken)
    {
        var validator = dto.Validate();
        if (!validator.IsValid) throw new ArgumentException(string.Join(",", validator.Errors.Select(e => e.ErrorMessage)));

        var duplicatedPosts = await _repository.GetAsync(p => p.UserId == dto.UserId && p.Title == dto.Title && p.Title == dto.Content).ToListAsync();
        if (duplicatedPosts.Count > 0) throw new ArgumentException("Post was already created");

        var post = new Post(dto.Title, dto.Content, dto.BannerUrl, dto.UserId);
        _repository.Add(post);

        if (!await _repository.CommitAsync(cancellationToken)) throw new ApplicationException("Was not possible to save the post");

        var user = await _userRepository.GetByIdAsync(post.UserId.ToString());
        return MapToGetPostDTO(post, user!);
    }

    public async Task<GetPostDTO> Update(Guid id, AddPostDTO dto, CancellationToken cancellationToken)
    {
        var validator = dto.Validate();
        if (!validator.IsValid) throw new ArgumentException(string.Join(",", validator.Errors.Select(e => e.ErrorMessage)));

        var post = await _repository.GetByIdAsync(id) ?? throw new ArgumentException("Post not found");
        if (post!.UserId != dto.UserId) throw new UnauthorizedAccessException();

        var duplicatedPosts = await _repository.GetAsync(p => p.UserId == dto.UserId && p.Title == dto.Title && p.Content == dto.Content).ToListAsync();
        if (duplicatedPosts.Count > 0) throw new ArgumentException("Post was already exists");

        post.Update(dto.Title, dto.Content, dto.BannerUrl);
        _repository.Update(post);

        if (!await _repository.CommitAsync(cancellationToken)) throw new ApplicationException("Was not possible to save the post");

        var user = await _userRepository.GetByIdAsync(post.UserId.ToString());
        return MapToGetPostDTO(post, user!);
    }

    public async Task Delete(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        var post = await _repository.GetByIdAsync(id) ?? throw new ArgumentException("Post not found");
        if (post.UserId != userId) throw new UnauthorizedAccessException();

        _repository.Delete(post);
        if (!await _repository.CommitAsync(cancellationToken)) throw new ApplicationException("Was not possible to delete the post");
    }

    public async Task<GetCommentDTO> Comment(Guid id, AddCommentDTO dto, CancellationToken cancellationToken)
    {
        var validator = dto.Validate();
        if (!validator.IsValid)
            throw new ArgumentException(string.Join(",", validator.Errors.Select(e => e.ErrorMessage)));

        var post = await _repository.GetByIdAsync(id) ?? throw new ArgumentException("Post not found");

        var comment = new Comment(dto.Content, dto.UserId);
        post.AddComment(comment);
        _repository.Update(post);

        if (!await _repository.CommitAsync(cancellationToken)) throw new ApplicationException("Was not possible to save the comment");

        var user = await _userRepository.GetByIdAsync(post.UserId.ToString());
        return MapToGetCommentDTO(comment, user!);
    }

    private static GetPostDTO MapToGetPostDTO(Post post, User user) =>
        new(post.Id, post.Title, post.Content, post.BannerUrl, user.FullName!, post.CreatedAt);

    private static GetPostByIdDTO MapToGetPostByIdDTO(Post post, User user) =>
        new(post.Id, post.Title, post.Content, post.BannerUrl, user.FullName!, post.CreatedAt, post.UserId);

    private static GetCommentDTO MapToGetCommentDTO(Comment comment, User user) =>
        new(comment.Id, comment.Content, comment.CreatedAt, user.FullName!, Guid.Parse(user.Id));
}
