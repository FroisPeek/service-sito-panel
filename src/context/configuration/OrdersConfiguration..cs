using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSitoPanel.src.model;

namespace ServiceSitoPanel.src.context.Configurations
{
    public class OrdersConfiguration : IEntityTypeConfiguration<Orders>
    {
        public void Configure(EntityTypeBuilder<Orders> entity)
        {
            entity.Property(e => e.date_order)
                .HasColumnType("timestamp without time zone");

            entity.Property(e => e.date_creation_order)
                .HasColumnType("timestamp without time zone");

            entity.Property(e => e.date_purchase_order)
                .HasColumnType("timestamp without time zone");

            entity.Property(e => e.date_conference)
                .HasColumnType("timestamp without time zone");

            entity.HasOne(o => o.ClientJoin)
                .WithMany()
                .HasForeignKey(o => o.client);
        }
    }
}
