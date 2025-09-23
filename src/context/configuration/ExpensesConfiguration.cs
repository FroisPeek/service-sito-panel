using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceSitoPanel.src.model;

namespace ServiceSitoPanel.src.context.Configurations
{
    public class ExpensesConfiguration : IEntityTypeConfiguration<Expenses>
    {
        public void Configure(EntityTypeBuilder<Expenses> entity)
        {
            entity.Property(e => e.expense_date)
                .HasColumnType("timestamp without time zone");

            entity.Property(e => e.description)
                .HasColumnType("text");

            entity.Property(e => e.price)
                .HasColumnType("numeric(10,2)");

            entity.Property(e => e.performed_at)
                .HasColumnType("timestamp without time zone");

            entity.Property(e => e.processed_at)
                .HasColumnType("timestamp without time zone")
                .IsRequired(false);

            entity.Property(e => e.payment_date)
                .HasColumnType("timestamp without time zone")
                .IsRequired(false);

            entity.Property(e => e.tenant_id)
                .HasColumnType("int");
        }
    }
}
