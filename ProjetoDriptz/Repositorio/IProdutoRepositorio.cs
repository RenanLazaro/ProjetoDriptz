using ProjetoDriptz.Models;

namespace ProjetoDriptz.Repositorio
{
    public interface IProdutoRepositorio
    {
        ProdutoModel ListarPorIId(int id);
        List<ProdutoModel>BuscarTodos();
        ProdutoModel Adicionar(ProdutoModel produto);

        ProdutoModel Editar(ProdutoModel produto);
        bool Excluir(int id);

    }
}
