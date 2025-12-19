using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoDriptz.Models;

namespace ProjetoDriptz.Data.Map
{


        public class VendaItemMap : IEntityTypeConfiguration<VendaItemModel>
        {
            public void Configure(EntityTypeBuilder<VendaItemModel> builder)
            {
                builder.ToTable("VendasItens");

                // Relacionamento com Produto - CORRIGIDO
                builder.HasOne(v => v.Produto)
                       .WithMany(p => p.VendaItems)  // <- AQUI
                       .HasForeignKey(v => v.ProdutoId)
                       .OnDelete(DeleteBehavior.Restrict);

                // Relacionamento com Estoque - CORRIGIDO
                builder.HasOne(v => v.Estoque)
                       .WithMany(e => e.VendasItens)  // <- AQUI
                       .HasForeignKey(v => v.EstoqueId)
                       .OnDelete(DeleteBehavior.Restrict);

                // Relacionamento com Estoque - CORRIGIDO
                builder.HasOne(v => v.Venda)
                       .WithMany(e => e.VendaItens)  // <- AQUI
                       .HasForeignKey(v => v.VendaId)
                       .OnDelete(DeleteBehavior.Restrict);

                builder.Property(v => v.Tamanho)
                       .IsRequired();

                builder.Property(v => v.Quantidade)
                       .IsRequired();



            }
        }

}
