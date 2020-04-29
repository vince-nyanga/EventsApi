using System.Threading.Tasks;

namespace EventsApi.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string emailAddress, string emailSubject, string emailBody);
    }
}
