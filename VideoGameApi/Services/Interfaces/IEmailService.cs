using VideoGameApi.Model;

namespace VideoGameApi.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(IEmailDto payload);
    }
}
