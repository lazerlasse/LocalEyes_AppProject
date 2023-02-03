using System.ComponentModel.DataAnnotations;

namespace LocalEyesWebAPI.Models
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }

        [DataType(DataType.DateTime), Display(Name = "Oprettet"), DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:MM}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [Required, Display(Name = "Besked")]
        public string MessageText { get; set; }

        [Required, Display(Name = "Adresse")]
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber), Display(Name = "Telefon")]
        public int? ReplyPhoneNumber { get; set; }

        [DataType(DataType.EmailAddress), Display(Name = "Email")]
        public string? ReplyMail { get; set; }
    }
}
