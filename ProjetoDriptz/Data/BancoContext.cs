using Microsoft.EntityFrameworkCore;
using ProjetoDriptz.Data.Map;

namespace ProjetoDriptz.Data
{
    public class BancoContext : DbContext
    {

        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        {
        }

        public DbSet<Models.ProdutoModel> Produtos { get; set; }
        public DbSet<Models.UsuarioModel> Usuarios { get; set; }
       
        public DbSet<Models.EstoqueModel> Estoques { get; set; }
        public DbSet<Models.VendaModel> Vendas { get; set; }
        public DbSet<Models.VendaItemModel> VendasItens { get; set; }


        //com relação entre produto e estoque

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EstoqueMap).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VendaMap).Assembly);

            base.OnModelCreating(modelBuilder);

        }


    }
}
