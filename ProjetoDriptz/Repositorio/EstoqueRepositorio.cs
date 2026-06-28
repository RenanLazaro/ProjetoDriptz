using Microsoft.EntityFrameworkCore;
using ProjetoDriptz.Data;
using ProjetoDriptz.Models;
using ProjetoDriptz.Repositorio.Interfaces;

namespace ProjetoDriptz.Repositorio
{
    public class EstoqueRepositorio : IEstoqueRepositorio
    {
        private readonly BancoContext _bancoContext;
        public EstoqueRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public EstoqueModel ListarPorIId(int id)
        {
           return _bancoContext.Estoques.FirstOrDefault(x => x.Id == id);
            
        }

        public List<EstoqueModel> ListarTodosComProduto()
        {
            return _bancoContext.Estoques
                .Include(e => e.Produto)
                .Where(e => e.Quantidade > 0)
                .ToList();
        }

        public List<EstoqueModel> BuscarTodos()
        {
            return _bancoContext.Estoques
                .Include(e => e.Produto)  // Carrega o produto relacionado
                .ToList();
        }
        public EstoqueModel BuscarPorProdutoETamanho(int produtoId, int tamanho)
        {
            // 🔥 REMOVA .AsNoTracking() se existir
            // 🔥 Use FirstOrDefault em vez de SingleOrDefault se houver duplicatas

            return _bancoContext.Estoques
                .Include(e => e.Produto) // Se precisar carregar o produto
                .FirstOrDefault(e => e.ProdutoId == produtoId && e.Tamanho == tamanho);
        }


        public List<EstoqueModel> BuscarPorProdutosETamanhos(List<int> produtoIds, List<int> tamanhos)
{
            return _bancoContext.Estoques
                .Include(e => e.Produto)
                .Where(e => produtoIds.Contains(e.ProdutoId) && tamanhos.Contains(e.Tamanho))
                .ToList();
        }

        public EstoqueModel BuscarPorProdutoId(int produtoId)
        {
            return _bancoContext.Estoques
                .FirstOrDefault(x => x.ProdutoId == produtoId);
        }

        public EstoqueModel Adicionar(EstoqueModel estoque)
        {

            //gravar no banco de dados
            
            _bancoContext.Estoques.Add(estoque);
            _bancoContext.SaveChanges();    

            return estoque;

        }

        public EstoqueModel Editar(EstoqueModel estoque)
        {
            EstoqueModel estoqueDb = _bancoContext.Estoques.FirstOrDefault(x => x.Id == estoque.Id);

            if (estoqueDb == null)
                throw new Exception("Houve um erro na atualização do produto.");

            estoqueDb.ProdutoId = estoque.ProdutoId;
            estoqueDb.Tamanho = estoque.Tamanho;
            estoqueDb.Cor = estoque.Cor;   
            estoqueDb.Quantidade = estoque.Quantidade;

            _bancoContext.Estoques.Update(estoqueDb);
            _bancoContext.SaveChanges();

            return estoqueDb;
        }


        public bool Excluir(int id)
        {
            EstoqueModel estoqueDb = ListarPorIId(id);

            if(estoqueDb == null) throw new Exception("Houve um erro na exclusão do produto em estoque!");

            _bancoContext.Estoques.Remove(estoqueDb);
            _bancoContext.SaveChanges();

            return true;
        }

     
    }
}
