using Microsoft.EntityFrameworkCore;
using ProjetoDriptz.Data;
using ProjetoDriptz.Models;
using ProjetoDriptz.Repositorio.Interfaces;

namespace ProjetoDriptz.Repositorio
{
    public class PedidoRepositorio : IPedidoRepositorio
    {
        private readonly BancoContext _context;

        public PedidoRepositorio(BancoContext context)
        {
            _context = context;
        }

        public PedidoModel Adicionar(PedidoModel pedido)
        {
            _context.Pedidos.Add(pedido);
            _context.SaveChanges();
            return pedido;
        }

        public PedidoModel BuscarPorId(int id)
        {
            return _context.Pedidos
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .FirstOrDefault(p => p.Id == id);
        }

        public List<PedidoModel> BuscarTodos()
        {
            return _context.Pedidos
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .OrderByDescending(p => p.DataPedido)
                .ToList();
        }

        public PedidoModel Editar(PedidoModel pedido)
        {
            var pedidoDb = _context.Pedidos.FirstOrDefault(p => p.Id == pedido.Id);
            if (pedidoDb == null) throw new Exception("Pedido não encontrado.");

            pedidoDb.Status = pedido.Status;
            _context.Pedidos.Update(pedidoDb);
            _context.SaveChanges();
            return pedidoDb;
        }
    }
}