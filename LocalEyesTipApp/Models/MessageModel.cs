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
        private string title;
        private string description;
        private string address;
        private int? replyPhoneNumber;
        private string? replyMail;
        private FileResult mediaFile;


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

        public string Title
        {
            get => title;
            set
            {
                if (title == value)
                    return;

                title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            }
        }

        public string Description
        {
            get => description;
            set
            {
                if (description == value)
                    return;

                description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
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

        public FileResult MediaFile
        {
            get => mediaFile;
            set
            {
                if (mediaFile == value)
                    return;

                mediaFile = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MediaFile)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
