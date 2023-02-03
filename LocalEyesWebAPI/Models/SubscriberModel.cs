using System.ComponentModel.DataAnnotations;

namespace LocalEyesWebAPI.Models
{
    public class SubscriberModel
    {
        [Key]
        public int SubscriberModelID { get; set; }

        [Required, Display(Name = "Navn")]
        public string SubscriberName { get; set; }

        [Required, Display(Name = "Pushover Key")]
        public string SubscriberPushoverKey { get; set; }

        [Display(Name = "Aktiv")]
        public bool SubscriberEnabled { get; set; }
    }
}
