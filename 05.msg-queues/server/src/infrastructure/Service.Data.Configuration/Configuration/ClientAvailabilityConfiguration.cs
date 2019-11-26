using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Data.Entities;

namespace Service.Data.Configuration.Configuration
{
    public class ClientAvailabilityConfiguration : IEntityTypeConfiguration<ClientAvailability>
    {
        public void Configure(EntityTypeBuilder<ClientAvailability> builder)
        {
            builder.ToTable("client_availability");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.ClientId)
                .HasColumnName("client_id")
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Message)
                .HasColumnName("message")
                .HasMaxLength(1024)
                .IsRequired();

            builder.Property(e => e.TimeStamp)
                .HasColumnName("timestamp")
                .HasColumnType("datetimeoffset(3)")
                .IsRequired();
        }
    }
}
