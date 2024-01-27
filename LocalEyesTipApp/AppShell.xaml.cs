using LocalEyesTipApp.Pages;

namespace LocalEyesTipApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            if (args.Current is not null)
            {
                ShellNavigatingDeferral token = args.GetDeferral();

                if (!args.Current.Location.OriginalString.Contains("SendTipPage"))
                {
                    token.Complete();
                }
                else
                {
                    var result = await DisplayActionSheet("Du er ved at forlade denne side, ønsker du at fortsætte?", "Nej", "Ja");
                    if (result != "Ja")
                    {
                        args.Cancel();
                    }
                    token.Complete();
                }
            }
        }
    }
}