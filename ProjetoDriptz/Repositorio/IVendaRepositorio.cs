using ProjetoDriptz.Models;
using ProjetoDriptz.Models.ViewModels;

namespace ProjetoDriptz.Repositorio
{
    public interface IVendaRepositorio
    {
        VendaModel ListarPorIId(int id);
        List<VendaModel> BuscarTodos();
        List<VendaModel> BuscarTodosComItens();
        VendaModel BuscarComItens(int id);
        VendaModel Adicionar(VendaModel venda);

        VendaModel Editar(VendaModel venda);
        bool Excluir(int id);

        void RemoverItem(VendaItemModel item);
        void Salvar();

    }
}
