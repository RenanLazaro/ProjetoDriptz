using ProjetoDriptz.Models;

namespace ProjetoDriptz.Repositorio
{
    public interface IPromocaoRepositorio
    {
        PromocaoModel Adicionar(PromocaoModel promocao);
        PromocaoModel BuscarPorId(int id);
        List<PromocaoModel> BuscarTodas();
        List<PromocaoModel> BuscarAtivasHoje();
        PromocaoModel? BuscarAtivaPorProduto(int produtoId);
        PromocaoModel Editar(PromocaoModel promocao);
        bool Excluir(int id);
    }
}