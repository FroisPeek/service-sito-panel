using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSitoPanel.src.model;

namespace ServiceSitoPanel.src.context.Configurations
{
    public class SolicitationsConfiguration : IEntityTypeConfiguration<Solicitations>
    {
        public void Configure(EntityTypeBuilder<Solicitations> entity)
        {
            entity.Property(e => e.orders)
                  .HasColumnType("integer[]");

            entity.Ignore(e => e.OrderJoin);

            entity.Property(e => e.date_solicitation)
                  .HasColumnType("timestamp with time zone");
        }
    }
}
