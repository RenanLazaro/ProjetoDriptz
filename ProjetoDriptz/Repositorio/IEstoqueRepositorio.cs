using ProjetoDriptz.Models;

namespace ProjetoDriptz.Repositorio
{
    public interface IEstoqueRepositorio
    {
        EstoqueModel ListarPorIId(int id);
        EstoqueModel BuscarPorProdutoId(int produtoId);
        List<EstoqueModel> BuscarTodos();
        EstoqueModel Adicionar(EstoqueModel produto);

        EstoqueModel Editar(EstoqueModel produto);
        bool Excluir(int id);

    }
}
