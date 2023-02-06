using LocalEyesWebAPI.Data;
using LocalEyesWebAPI.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;

namespace LocalEyesWebAPI.Services
{
    public class PushoverSender
    {
        private readonly Uri pushoverUri = new("https://api.pushover.net/1/messages.json");
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private readonly IDataProtector _pushoverDataProtector;
        private readonly IDataProtector _subscriberDataProtector;


        public PushoverSender(ILogger<PushoverSender> logger, ApplicationDbContext context, IDataProtectionProvider protectionProvider)
        {
            _logger = logger;
            _context = context;
            _pushoverDataProtector = protectionProvider.CreateProtector(nameof(PushoverSenderAPIModel));
            _subscriberDataProtector = protectionProvider.CreateProtector(nameof(SubscriberModel));
        }


        public async Task SendMessage(string message)
        {
            List<SubscriberModel> subscribers = new();
            List<string> userKeysList = new();
            subscribers = _context.Subscribers.Where(s => s.SubscriberEnabled == true).ToList();
            var apiKey = _pushoverDataProtector.Unprotect(_context.PushoverSenderAPIs.FirstOrDefault().PushoverSenderAPIKey);

            foreach (var subscriber in subscribers)
            {
                userKeysList.Add(_subscriberDataProtector.Unprotect(subscriber.SubscriberPushoverKey));
            }

            var keys = string.Join(",", userKeysList.ToArray());

            // Create parameters for the request to send.
            Dictionary<string, string> parameters = new()
            {
                { "token", apiKey },
                { "user", keys },
                { "message", message }
            };

            // Initialize new instance of the HttpClient.
            using HttpClient client = new();

            // Create the request message body.
            var req = new HttpRequestMessage(HttpMethod.Post, pushoverUri) { Content = new FormUrlEncodedContent(parameters) };

            try
            {
                // Try send the requst to Pushover servers async.
                var result = await client.SendAsync(req);

                // The request was sendt. Log result and return.
                _logger.LogInformation("Meddelelsen blev sendt til Pushover med status: " + result.ReasonPhrase);
                return;
            }
            catch (Exception ex)
            {
                // Log the error and return.
                _logger.LogError("Der opstod en uventet fejl i forsøget på at sende Pushover besked: ", ex.Message);
                return;
            }
        }
    }
}
