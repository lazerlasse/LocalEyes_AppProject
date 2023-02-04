using Microsoft.AspNetCore.Http;
using Microsoft.Maui.Storage;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace LocalEyesTipApp.Models
{
    public class MessageModel : INotifyPropertyChanged
    {
        // Private properties...
        private int messageId;
        private string messageText;
        private string address;
        private int? replyPhoneNumber;
        private string? replyMail;
        private IEnumerable<FileResult> mediaFiles;


        // Public properties...
        public int MessageId
        {
            get => messageId;
            set
            {
                if (messageId == value)
                    return;

                messageId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MessageId)));
            }
        }

        public string MessageText
        {
            get => messageText;
            set
            {
                if (messageText == value)
                    return;

                messageText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MessageText)));
            }
        }

        public string Address
        {
            get => address;
            set
            {
                if (address == value)
                    return;

                address = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Address)));
            }
        }

        public int? ReplyPhoneNumber
        {
            get => replyPhoneNumber;
            set
            {
                if (replyPhoneNumber == value)
                    return;

                replyPhoneNumber = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReplyPhoneNumber)));
            }
        }

        public string? ReplyMail
        {
            get => replyMail;
            set
            {
                if (replyMail == value)
                    return;

                replyMail = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReplyMail)));
            }
        }

        public IEnumerable<FileResult> MediaFiles
        {
            get => mediaFiles;
            set
            {
                if (mediaFiles == value)
                    return;

                mediaFiles = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MediaFiles)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
