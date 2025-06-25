using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Net.Mail;

namespace YumBlazor.Services
{
    public class GraphHelperService : IGraphHelperService
    {

        private readonly IConfiguration _config;
        private readonly GraphServiceClient _appClient;
        private readonly string serviceAccount;

        public GraphHelperService(IConfiguration config, IHostEnvironment hostEnvironment)
        {
            _config = config;
            var tenantId = _config["GraphHelper:tenantId"];
            var clientId = _config["GraphHelper:clientId"];
            var clientSecret = _config["GraphHelper:clientSecret"];
            serviceAccount = _config["GraphHelper:serviceAccount"];
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                throw new InvalidOperationException("Graph configuration is missing.");
            }

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            _appClient = new GraphServiceClient(credential, new[] { "https://graph.microsoft.com/.default" });
        }

        public async Task SendEmailAsync(string title, string body, string[]? recipients = null)
        {
            if (recipients == null || recipients.Length == 0)
            {
                recipients = LoadRecipientsFromSettings();
            }

            // Validate recipients
            var invalidEmails = recipients
                .Where(email => !IsValidEmail(email))
                .ToArray();

            if (invalidEmails.Length > 0)
            {
                throw new InvalidOperationException($"Invalid email address(es): {string.Join(", ", invalidEmails)}");
            }

            var toRecipients = recipients.Select(email => new Recipient
            {
                EmailAddress = new EmailAddress { Address = email }
            }).ToList();

            var message = new Message
            {
                Subject = title,
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = body
                },
                ToRecipients = toRecipients
            };

            var mailbody = new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
            {
                Message = message,
                SaveToSentItems = false
            };

            try
            {
                await _appClient.Users[serviceAccount]
                    .SendMail
                    .PostAsync(mailbody);
            }
            catch (Exception ex)
            {
                // Rethrow so your page can catch and show Snackbar
                throw;
            }
        }

        private string[] LoadRecipientsFromSettings()
        {
            var settingsFile = Path.Combine(AppContext.BaseDirectory, "usersettings.json");

            if (!File.Exists(settingsFile))
            {
                throw new FileNotFoundException($"Could not find {settingsFile}");
            }

            var json = File.ReadAllText(settingsFile);

            var doc = System.Text.Json.JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("emailRecipients", out var recipientsElement))
            {
                throw new InvalidOperationException("No 'emailRecipients' found in usersettings.json");
            }

            var recipients = recipientsElement.EnumerateArray()
                .Select(e => e.GetString())
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .ToArray();

            if (recipients.Length == 0)
            {
                throw new InvalidOperationException("No valid recipients in 'emailRecipients'");
            }

            return recipients!;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
