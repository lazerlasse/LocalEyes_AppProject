using System.ComponentModel.DataAnnotations;

namespace LocalEyesWebAPI.Models
{
    public class RecievedMessageModel
    {
        public string MessageText { get; set; }

        public string Address { get; set; }

        public string ReplyPhoneNumber { get; set; } = string.Empty;

        public string ReplyMail { get; set; } = string.Empty;

        public List<IFormFile> MediaFiles { get; set; } = new List<IFormFile>();
    }
}
