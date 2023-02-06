using LocalEyesTipApp.Models;

namespace LocalEyesTipApp.Pages;

public partial class LatestNewsPage : ContentPage
{
	public LatestNewsPage()
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