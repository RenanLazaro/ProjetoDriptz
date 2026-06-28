using System.ComponentModel.DataAnnotations;

namespace ProjetoDriptz.Enums
{
    public enum TipoProduto
    {
        [Display(Name = "Pastel")] Pastel = 0,
        [Display(Name = "Salgado")] Salgado = 1,
        [Display(Name = "Bebida")] Bebida = 2,
        [Display(Name = "Sobremesa")] Sobremesa = 3,
        [Display(Name = "Combo")] Combo = 4,
        [Display(Name = "Outros")] Outros = 5
    }

    /*    public enum TamanhoProduto
        {
            [Display(Name = "P")] P = 0,
            [Display(Name = "M")] M = 1,
            [Display(Name = "G")] G = 2,
            [Display(Name = "GG")] GG = 3
        }*/

    public enum TamanhoProduto
    {
        [Display(Name = "P")] P = 0,
        [Display(Name = "M")] M = 1,
        [Display(Name = "G")] G = 2,
        [Display(Name = "GG")] GG = 3,
        [Display(Name = "XG")] XG = 4,
        [Display(Name = "EG")] EG = 5,
        [Display(Name = "EXG")] EXG = 6
    }

    public enum PerfilUsuario
    {
        [Display(Name = "Administrador")] Administrador = 0,
        [Display(Name = "Padrão")] Padrao = 1,
        [Display(Name = "Cliente")] Cliente = 2,

    }


    public enum FormaDePagamento
    {
        [Display(Name = "Dinheiro")] Dinheiro = 0,
        [Display(Name = "Pix")] Pix = 1,
        [Display(Name = "Cartão de Crédito/Débito")] Cartao = 2,

    }

    public enum TipoEntrega
    {
        [Display(Name = "Retirada no local")] Retirada = 0,
        [Display(Name = "Delivery")] Delivery = 1,
    }

    public enum StatusPedido
    {
        [Display(Name = "Aguardando confirmação")] Aguardando = 0,
        [Display(Name = "Confirmado")] Confirmado = 1,
        [Display(Name = "Em preparo")] EmPreparo = 2,
        [Display(Name = "Saiu para entrega")] SaiuParaEntrega = 3,
        [Display(Name = "Pronto para retirada")] ProntoParaRetirada = 4,
        [Display(Name = "Entregue")] Entregue = 5,
        [Display(Name = "Cancelado")] Cancelado = 6,
    }

    public enum TipoDesconto
    {
        [Display(Name = "Percentual (ex: 10% off)")] Percentual = 0,
        [Display(Name = "Preço fixo (ex: por R$20)")] PrecoFixo = 1,
    }
}
