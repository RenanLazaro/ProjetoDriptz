using ProjetoDriptz.Models;

namespace ProjetoDriptz.Repositorio.Interfaces
{
    public interface IEstoqueRepositorio
    {
        EstoqueModel ListarPorIId(int id);
        EstoqueModel BuscarPorProdutoETamanho(int produtoId, int tamanho);
        List<EstoqueModel> BuscarPorProdutosETamanhos(List<int> produtoIds, List<int> tamanhos);
        EstoqueModel BuscarPorProdutoId(int produtoId);
        List<EstoqueModel> BuscarTodos();
        List<EstoqueModel> ListarTodosComProduto();
        EstoqueModel Adicionar(EstoqueModel produto);

        EstoqueModel Editar(EstoqueModel produto);
        bool Excluir(int id);

    }
}
