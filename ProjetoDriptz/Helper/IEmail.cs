namespace ProjetoDriptz.Helper
{
    public interface IEmail
    {

        bool Enviar(string emailDestino, string assunto, string mensagem);
    }
}
