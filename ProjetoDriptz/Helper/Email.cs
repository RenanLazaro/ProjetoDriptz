using System.Net.Mail;

namespace ProjetoDriptz.Helper
{
    public class Email : IEmail
    {
        private readonly IConfiguration _configuration;
        
        
        public Email(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool Enviar(string emailDestino, string assunto, string mensagem)
        {
            try
            {
                string host = _configuration.GetValue<string>("SMTP:Host");        
                string nome = _configuration.GetValue<string>("SMTP:Nome");
                string userName = _configuration.GetValue<string>("SMTP:UserName");
                string senha = _configuration.GetValue<string>("SMTP:Senha");
                int porta = _configuration.GetValue<int>("SMTP:Porta");

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(userName, nome)
                };

                mail.To.Add(emailDestino);
                mail.Subject = assunto;
                mail.Body = mensagem;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient(host, porta))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(userName, senha);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);

                    return true;
                }


            }
            catch (Exception ex)
            {
                //gravar log de erro
                return false;
            }


        }
    }
}
