using Client.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Client.Data.Configuration.Configuration
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.ToTable("resource");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(e => e.Path)
                .HasColumnName("path")
                .HasMaxLength(1024)
                .IsRequired();

            builder.Property(e => e.Size)
                .HasColumnName("size")
                .IsRequired();

            builder.Property(e => e.Hash)
                .HasMaxLength(64)
                .IsRequired();
        }
    }
}
