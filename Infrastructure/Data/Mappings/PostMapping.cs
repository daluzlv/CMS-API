using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Mappings;

public class PostMapping : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Content).IsRequired();
        builder.Property(p => p.UserId).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired();
        builder.HasMany(p => p.Comments).WithOne();
    }
}
