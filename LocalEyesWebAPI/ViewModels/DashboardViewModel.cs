using LocalEyesWebAPI.Models;

namespace LocalEyesWebAPI.ViewModels
{
    public class DashboardViewModel
    {
        public IEnumerable<PushoverSenderAPIModel> PushoverSenderAPI { get; set; }
        public IEnumerable<SubscriberModel> Subscriber { get; set; }
    }
}
