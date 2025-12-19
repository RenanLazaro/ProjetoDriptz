using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoDriptz.Models;

namespace ProjetoDriptz.Data.Map
{
    public class VendaMap : IEntityTypeConfiguration<VendaModel>
    {
        public void Configure(EntityTypeBuilder<VendaModel> builder)
        {
            builder.ToTable("Vendas");

            builder.Property(v => v.FormaDePagamentoAdicional)
         .IsRequired(false);

            builder.Property(v => v.ValorAdicional);
        }
    }
}