using System.ComponentModel.DataAnnotations;

namespace ProjetoDriptz.Enums
{
    public enum TipoProduto
    {
        [Display(Name = "Camisa")] Camisa = 0,
        [Display(Name = "Blusa de Frio")] BlusaDeFrio = 1,
        [Display(Name = "Bermuda")] Bermuda = 2,
        [Display(Name = "Boné")] Boné = 3
    }

    public enum TamanhoProduto
    {
        [Display(Name = "P")] P = 0,
        [Display(Name = "M")] M = 1,
        [Display(Name = "G")] G = 2,
        [Display(Name = "GG")] GG = 3
    }

    public enum PerfilUsuario
    {
        [Display(Name = "Administrador")] Administrador = 0,
        [Display(Name = "Padrão")] Padrao = 1,

    }


    public enum FormaDePagamento
    {
        [Display(Name = "Dinheiro")] Dinheiro = 0,
        [Display(Name = "Pix")] Pix = 1,
        [Display(Name = "Cartão de Crédito/Débito")] Cartao = 2,

    }
}
