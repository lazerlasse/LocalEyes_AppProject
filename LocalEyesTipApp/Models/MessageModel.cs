using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.Net.Mail;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;

namespace LocalEyesTipApp.Models
{
    public class MessageModel
    {
        public int MessageId { get; set; }
        public string MessageText { get; set; }
        public string Address { get; set; }
        public int? ReplyPhoneNumber { get; set; }
        public string ReplyMail { get; set; }
        public List<FileResult> MediaFiles { get; set; } = new();
    }
}
