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

            // Relacionamento com Produto - CORRIGIDO
            builder.HasOne(v => v.Produto)
                   .WithMany(p => p.Vendas)  // <- AQUI
                   .HasForeignKey(v => v.ProdutoId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento com Estoque - CORRIGIDO
            builder.HasOne(v => v.Estoque)
                   .WithMany(e => e.Vendas)  // <- AQUI
                   .HasForeignKey(v => v.EstoqueId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(v => v.Tamanho)
                   .IsRequired();

            builder.Property(v => v.Quantidade)
                   .IsRequired();

            builder.Property(v => v.PrecoItem)
                   .IsRequired();

            builder.Property(v => v.FormaDePagamentoAdicional)
         .IsRequired(false);

            builder.Property(v => v.ValorAdicional)
          .IsRequired(false);
        }
    }
}