using ProjetoDriptz.Models;

namespace ProjetoDriptz.Repositorio
{
    public interface IVendaRepositorio
    {
        VendaModel ListarPorIId(int id);
        List<VendaModel> BuscarTodos();
        VendaModel Adicionar(VendaModel venda);

        VendaModel Editar(VendaModel venda);
        bool Excluir(int id);

    }
}
