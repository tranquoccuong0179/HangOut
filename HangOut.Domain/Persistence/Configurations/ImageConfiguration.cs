using HangOut.Domain.Entities;
using HangOut.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HangOut.Domain.Persistence.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Url)
            .IsRequired();

        builder.Property(i => i.IsMain)
            .IsRequired();

        builder.Property(i => i.ImageType)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (EImageType)Enum.Parse(typeof(EImageType), v)
            );
        
        builder.HasOne(i => i.Business)
            .WithMany(b => b.Images)
            .HasForeignKey(i => i.ObjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Event)
            .WithMany(e => e.Images)
            .HasForeignKey(i => i.ObjectId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}