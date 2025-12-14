using Microsoft.EntityFrameworkCore;
using ProjetoDriptz.Data;
using ProjetoDriptz.Models;

namespace ProjetoDriptz.Repositorio
{
    public class VendaRepositorio : IVendaRepositorio
    {
        private readonly BancoContext _bancoContext;
        public VendaRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public VendaModel ListarPorIId(int id)
        {
         //  return _bancoContext.Estoques.FirstOrDefault(x => x.Id == id);
            return _bancoContext.Vendas
                .Include(e => e.Produto)  // Carrega o produto relacionado
                .FirstOrDefault(x => x.Id == id);
        }

        public List<VendaModel> BuscarTodos()
        {
            return _bancoContext.Vendas
                .Include(e => e.Produto)  // Carrega o produto relacionado
                .ToList();
        }


        public VendaModel Adicionar(VendaModel venda)
        {

            //gravar no banco de dados
            
            _bancoContext.Vendas.Add(venda);
            _bancoContext.SaveChanges();    

            return venda;

        }

        public VendaModel Editar(VendaModel venda)
        {
            VendaModel vendaDb = _bancoContext.Vendas.FirstOrDefault(x => x.Id == venda.Id);

            if (vendaDb == null)
                throw new Exception("Houve um erro na atualização do produto.");

            vendaDb.ProdutoId = venda.ProdutoId;
            vendaDb.Tamanho = venda.Tamanho;
           // vendaDb.Cor = venda.Cor;
            vendaDb.Quantidade = venda.Quantidade;

            _bancoContext.Vendas.Update(vendaDb);
            _bancoContext.SaveChanges();

            return vendaDb;
        }


        public bool Excluir(int id)
        {
            VendaModel vendaDb = ListarPorIId(id);

            if(vendaDb == null) throw new Exception("Houve um erro na exclusão do produto em estoque!");

            _bancoContext.Vendas.Remove(vendaDb);
            _bancoContext.SaveChanges();

            return true;
        }

        public EstoqueModel Adicionar(EstoqueModel produto)
        {
            throw new NotImplementedException();
        }

        public EstoqueModel Editar(EstoqueModel produto)
        {
            throw new NotImplementedException();
        }
    }
}
