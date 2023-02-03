using EllipticCurve.Utils;
using LocalEyesWebAPI.Data;
using LocalEyesWebAPI.Models;
using LocalEyesWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace LocalEyesWebAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageAPIController : ControllerBase
    {
        private readonly ILogger<MessageAPIController> _logger;
        private readonly UploadFileHandler _uploadFileHandler;
        private readonly ApplicationDbContext _context;
        private readonly PushoverSender _pushoverSender;

        public MessageAPIController(ILogger<MessageAPIController> logger, UploadFileHandler uploadFileHandler, ApplicationDbContext context, PushoverSender pushoverSender)
        {
            _logger = logger;
            _uploadFileHandler = uploadFileHandler;
            _context = context;
            _pushoverSender = pushoverSender;
        }

        // GET: Test.
        [HttpGet]
        public IActionResult Test()
        {
            _logger.LogInformation("API Controlleren modtog en GET anmodning...");

            return Ok("Anmodning modtaget...");
        }

        // POST: api/MessageAPIController/SendMessage
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromForm] RecievedMessageModel messageRecieved)
        {
            // Create a string builder to build the final message to send.
            var messageToSend = new StringBuilder();

            // Create new message from recieved data.
            var message = new Message()
            {
                MessageText = messageRecieved.MessageText,
                Address = messageRecieved.Address,
                ReplyPhoneNumber = int.Parse(messageRecieved.ReplyPhoneNumber),
                ReplyMail = messageRecieved.ReplyMail

            };

            try
            {
                // Try to save the message to db.
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Der opstod en uventet fejl i forsøget på at gemme meddelelsen i databasen: ", ex.Message);
                return BadRequest(ex.Message);
            }

            // If the recived data contains media files save these files to disk.
            if (messageRecieved.MediaFiles != null)
            {
                // Save files to disk.
                bool success = await _uploadFileHandler.SaveUploadAsync(messageRecieved.MediaFiles, message.MessageID);

                // Check if files was saved succesful.
                if (success)
                {
                    // Generate link to attacted media file.
                    var links = MediaLinkGenerator.GenerateLink(message.MessageID);

                    // Form the massage and send with pushover.
                    messageToSend.AppendLine("Tip:");
                    messageToSend.AppendLine(message.MessageText);
                    messageToSend.AppendLine();
                    messageToSend.AppendLine("Adresse:");
                    messageToSend.AppendLine(message.Address);
                    messageToSend.AppendLine();
                    messageToSend.AppendLine("Kontakt oplysninger:");
                    messageToSend.AppendLine("Tlf." + message.ReplyPhoneNumber.ToString());
                    messageToSend.AppendLine("Mail." + message.ReplyMail);
                    messageToSend.AppendLine();
                    messageToSend.AppendLine("Vedhæftede filer:");

                    foreach (var link in links)
                    {
                        messageToSend.AppendLine(link);
                    }

                    // Send message to pushover subscribers...
                    await _pushoverSender.SendMessage(messageToSend.ToString());

                    // Write info to log.
                    _logger.LogInformation("Beskeden blev modtaget med succes, sammen med den/de vedhæftede fil'er': ", messageRecieved.MediaFiles.ToArray().ToString());

                    // Return
                    return Ok("Beskeden blev modtaget med succes, sammen med den vedhæftede fil: " + messageRecieved.MediaFiles.ToArray().ToString());
                }
                else
                {
                    // Form the massage and send with pushover.
                    messageToSend.AppendLine("Tip:");
                    messageToSend.AppendLine(message.MessageText);
                    messageToSend.AppendLine();
                    messageToSend.AppendLine("Adresse:");
                    messageToSend.AppendLine(message.Address);
                    messageToSend.AppendLine();
                    messageToSend.AppendLine("Kontakt oplysninger:");
                    messageToSend.AppendLine("Tlf." + message.ReplyPhoneNumber.ToString());
                    messageToSend.AppendLine("Mail." + message.ReplyMail);
                    messageToSend.AppendLine();
                    messageToSend.AppendLine("Fejl meddelelse:");
                    messageToSend.AppendLine("Der var vedhæftet filer sammen med beskeden, men der opstod en uventet fejl under forsøget på at behandle filerne. Kontakt support info@fixitmedia.dk");

                    // Send message to pushover subscribers...
                    await _pushoverSender.SendMessage(messageToSend.ToString());

                    // Write warning to log.
                    _logger.LogWarning("Beskeden blev modtaget med succes, men den der opstod en fejl i forsøget på at gemme den/de vedhædtede fil'er'! ", messageRecieved.MediaFiles.ToArray().ToString());

                    // Return.
                    return Ok("Beskeden blev modtaget med succes, men den der opstod en fejl i forsøget på at gemme den vedhædtede fil! " + messageRecieved.MediaFiles.ToArray().ToString());
                }
            }

            // Form the massage and send with pushover.
            messageToSend.AppendLine("Tip:");
            messageToSend.AppendLine(message.MessageText);
            messageToSend.AppendLine();
            messageToSend.AppendLine("Adresse:");
            messageToSend.AppendLine(message.Address);
            messageToSend.AppendLine();
            messageToSend.AppendLine("Kontakt oplysninger:");
            messageToSend.AppendLine("Tlf." + message.ReplyPhoneNumber.ToString());
            messageToSend.AppendLine("Mail." + message.ReplyMail);

            // Send message to pushover subscribers...
            await _pushoverSender.SendMessage(messageToSend.ToString());

            // Log info to log.
            _logger.LogInformation("Besked uden vedhæftet filer blev modtaget og sendt med pushover.");

            // Return.
            return Ok("Besked uden vedhæftet filer blev modtaget med succes.");
        }
    }
}
