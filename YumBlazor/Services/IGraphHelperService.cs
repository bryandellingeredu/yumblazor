namespace YumBlazor.Services
{
    public interface IGraphHelperService
    {
        Task SendEmailAsync(string title, string body, string[]? recipients = null);
    }
}
