using LocalEyesTipApp.Pages;

namespace LocalEyesTipApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LatestNewsPage), typeof(LatestNewsPage));
            Routing.RegisterRoute(nameof(SendTipPage), typeof(SendTipPage));
            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
        }
    }
}