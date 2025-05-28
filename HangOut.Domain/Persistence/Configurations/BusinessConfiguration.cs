using HangOut.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HangOut.Domain.Persistence.Configurations;

public class BusinessConfiguration : IEntityTypeConfiguration<Business>
{
    public void Configure(EntityTypeBuilder<Business> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(b => b.Active)
            .IsRequired();

        builder.Property(b => b.Vibe)
            .HasMaxLength(500);

        builder.Property(b => b.Latitude)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(b => b.Longitude)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(b => b.Province)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(b => b.Description)
            .HasMaxLength(1000);

        builder.HasOne(b => b.Account)
            .WithMany(a => a.Businesses)
            .HasForeignKey(b => b.AccountId)
            .OnDelete(DeleteBehavior.Restrict);


    }
}