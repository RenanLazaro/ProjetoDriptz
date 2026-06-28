using ProjetoDriptz.Models;

namespace ProjetoDriptz.Repositorio
{
    public interface IPedidoRepositorio
    {
        PedidoModel Adicionar(PedidoModel pedido);
        PedidoModel BuscarPorId(int id);
        List<PedidoModel> BuscarTodos();
        PedidoModel Editar(PedidoModel pedido);
    }
}