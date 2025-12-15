using ProjetoDriptz.Data;
using ProjetoDriptz.Models;

namespace ProjetoDriptz.Repositorio
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly BancoContext _bancoContext;
        public ProdutoRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public ProdutoModel ListarPorIId(int id)
        {
           return _bancoContext.Produtos.FirstOrDefault(x => x.Id == id);
            
        }

        public List<ProdutoModel> BuscarTodos()
        {             
            return _bancoContext.Produtos.ToList();
        }

        public ProdutoModel Adicionar(ProdutoModel produto)
        {

            //gravar no banco de dados
            
            _bancoContext.Produtos.Add(produto);
            _bancoContext.SaveChanges();    

            return produto;

        }

        public ProdutoModel Editar(ProdutoModel produto)
        {
            ProdutoModel produtoDb = _bancoContext.Produtos
                .FirstOrDefault(x => x.Id == produto.Id);

            if (produtoDb == null)
                throw new Exception("Houve um erro na atualização do produto.");

            produtoDb.NomeProduto = produto.NomeProduto;
            produtoDb.Tipo = produto.Tipo;
            produtoDb.PrecoCusto = produto.PrecoCusto;
            produtoDb.Preco = produto.Preco;

            _bancoContext.Produtos.Update(produtoDb);
            _bancoContext.SaveChanges();

            return produtoDb;
        }


        //   public ProdutoModel Excluir(ProdutoModel produto)
        //   {
        //     _bancoContext.Produtos.Remove(produto);
        //     _bancoContext.SaveChanges();

        //            return produto;
        //       }

        public bool Excluir(int id)
        {
          ProdutoModel produtoDb = ListarPorIId(id);

            if(produtoDb == null) throw new Exception("Houve um erro na exclusão do produto!");

            _bancoContext.Produtos.Remove(produtoDb);
            _bancoContext.SaveChanges();

            return true;
        }
    }
}
