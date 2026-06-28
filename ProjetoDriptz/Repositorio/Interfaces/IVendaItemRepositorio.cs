using ProjetoDriptz.Models;

namespace ProjetoDriptz.Repositorio.Interfaces
{
    public interface IVendaItemRepositorio
    {
        VendaItemModel Adicionar(VendaItemModel venda);
        bool Excluir(int id);
    }
}
