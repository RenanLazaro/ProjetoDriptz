using System.Security.Cryptography;

namespace ProjetoDriptz.Helper
{
    public static class Criptografia
    {
        public static string GerarHash(this string senha)
        {
            //gerar um hash para a senha
            //utilizando o algoritmo SHA256
            using (var sha1 = SHA1.Create())
            {
                //converter a senha em um array de bytes
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(senha);
                //gerar o hash
                byte[] hash = sha1.ComputeHash(bytes);
                //converter o hash em uma string hexadecimal
                return Convert.ToHexString(hash);
            }
        }
    }
}
