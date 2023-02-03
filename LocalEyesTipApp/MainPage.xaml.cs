using LocalEyesTipApp.Interfaces;
using LocalEyesTipApp.Models;
using LocalEyesTipApp.Pages;

namespace LocalEyesTipApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnSendTipBtnClicked(object sender, EventArgs e)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "MessageModel", new MessageModel() }
            };

            await Shell.Current.GoToAsync(nameof(SendTipPage), navigationParameter);
        }
    }
}