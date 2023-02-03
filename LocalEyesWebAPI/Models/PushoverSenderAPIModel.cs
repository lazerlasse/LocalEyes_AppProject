using System.ComponentModel.DataAnnotations;

namespace LocalEyesWebAPI.Models
{
    public class PushoverSenderAPIModel
    {
        [Key]
        public int PushoverSenderAPIID { get; set; }

        [Display(Name = "API Navn"), Required]
        public string PushoverSenderName { get; set; }

        [Display(Name = "API Key"), Required]
        public string PushoverSenderAPIKey { get; set; }
    }
}
