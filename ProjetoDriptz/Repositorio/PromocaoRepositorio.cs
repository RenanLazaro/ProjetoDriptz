// =====================================================================
// ARQUIVO: Repositorio/PromocaoRepositorio.cs  — ARQUIVO NOVO
// =====================================================================
using Microsoft.EntityFrameworkCore;
using ProjetoDriptz.Data;
using ProjetoDriptz.Models;

namespace ProjetoDriptz.Repositorio
{
    public class PromocaoRepositorio : IPromocaoRepositorio
    {
        private readonly BancoContext _context;

        public PromocaoRepositorio(BancoContext context)
        {
            _context = context;
        }

        public PromocaoModel Adicionar(PromocaoModel promocao)
        {
            _context.Promocoes.Add(promocao);
            _context.SaveChanges();
            return promocao;
        }

        public PromocaoModel BuscarPorId(int id)
        {
            return _context.Promocoes
                .Include(p => p.Produto)
                .FirstOrDefault(p => p.Id == id);
        }

        public List<PromocaoModel> BuscarTodas()
        {
            return _context.Promocoes
                .Include(p => p.Produto)
                .OrderByDescending(p => p.DataInicio)
                .ToList();
        }

        public List<PromocaoModel> BuscarAtivasHoje()
        {
            var hoje = DateTime.Today;
            return _context.Promocoes
                .Include(p => p.Produto)
                .Where(p => p.Ativo && p.DataInicio <= hoje && hoje <= p.DataFim)
                .ToList();
        }

        public PromocaoModel? BuscarAtivaPorProduto(int produtoId)
        {
            var hoje = DateTime.Today;
            return _context.Promocoes
                .FirstOrDefault(p =>
                    p.ProdutoId == produtoId &&
                    p.Ativo &&
                    p.DataInicio <= hoje &&
                    hoje <= p.DataFim);
        }

        public PromocaoModel Editar(PromocaoModel promocao)
        {
            var db = _context.Promocoes.FirstOrDefault(p => p.Id == promocao.Id);
            if (db == null) throw new Exception("Promoção não encontrada.");

            db.Nome = promocao.Nome;
            db.ProdutoId = promocao.ProdutoId;
            db.TipoDesconto = promocao.TipoDesconto;
            db.DescontoPercentual = promocao.DescontoPercentual;
            db.PrecoFixo = promocao.PrecoFixo;
            db.DataInicio = promocao.DataInicio;
            db.DataFim = promocao.DataFim;
            db.Ativo = promocao.Ativo;

            _context.Promocoes.Update(db);
            _context.SaveChanges();
            return db;
        }

        public bool Excluir(int id)
        {
            var db = _context.Promocoes.FirstOrDefault(p => p.Id == id);
            if (db == null) throw new Exception("Promoção não encontrada.");
            _context.Promocoes.Remove(db);
            _context.SaveChanges();
            return true;
        }
    }
}