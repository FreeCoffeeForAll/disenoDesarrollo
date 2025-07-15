using System.Threading.Tasks;

namespace ProyectoFinalDiseño.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlMessage);
    }
}
