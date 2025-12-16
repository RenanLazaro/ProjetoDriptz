using ProjetoDriptz.Data;
using ProjetoDriptz.Models;

namespace ProjetoDriptz.Repositorio
{
    public class VendaItemRepositorio : IVendaItemRepositorio
    {
        private readonly BancoContext _bancoContext;
        public VendaItemRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public VendaItemModel Adicionar(VendaItemModel vendaItem)
        {

            //gravar no banco de dados

            _bancoContext.VendasItens.Add(vendaItem);
            _bancoContext.SaveChanges();

            return vendaItem;

        }
    }
}
