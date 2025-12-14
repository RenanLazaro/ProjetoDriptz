using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjetoDriptz.Models;

namespace ProjetoDriptz.Data.Map
{
    public class EstoqueMap : IEntityTypeConfiguration<EstoqueModel>
    {

        public void Configure(EntityTypeBuilder<EstoqueModel> builder)
        {
            builder.ToTable("Estoques");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Produto);

        }
    }
}
