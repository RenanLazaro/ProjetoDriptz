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

        public bool Excluir(int id)
        {
            VendaItemModel vendaDb = ListarPorIId(id);

            if (vendaDb == null) throw new Exception("Houve um erro na exclusão do produto em estoque!");

            _bancoContext.VendasItens.Remove(vendaDb);
            _bancoContext.SaveChanges();

            return true;
        }


        public VendaItemModel ListarPorIId(int id)
        {
            //  return _bancoContext.Estoques.FirstOrDefault(x => x.Id == id);
            return _bancoContext.VendasItens
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
