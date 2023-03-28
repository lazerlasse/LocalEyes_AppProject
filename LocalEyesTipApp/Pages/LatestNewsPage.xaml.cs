using LocalEyesTipApp.Models;
using System.Windows.Input;

namespace LocalEyesTipApp.Pages;

public partial class LatestNewsPage : ContentPage
{
    private bool isRefreshing;
    private bool isLoading;

    public Command RefreshCommand { get; }

    public LatestNewsPage()
    {
        InitializeComponent();
        IsLoading = false;
        RefreshCommand = new Command(RefreshWebView);
        LocaleyesLatestNewsWebView.Source = "https://localeyes.dk/category/agency/";
        BindingContext = this;
    }

    public bool IsRefreshing
    {
        get => isRefreshing;
        set
        {
            if (isRefreshing == value)
                return;

            isRefreshing = value;
            OnPropertyChanged(nameof(IsRefreshing));
        }
    }

    public bool IsLoading
    {
        get => IsLoading;
        set
        {
            if(isLoading == value)
                return;

            isLoading = value;
            OnPropertyChanged(nameof(IsLoading));
        }
    }

    async void OnSendTipBtnClicked(object sender, EventArgs e)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { "Message", new MessageModel() }
        };

        await Shell.Current.GoToAsync(nameof(SendTipPage), navigationParameter);
    }

    void WebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
        if(IsLoading is true)
            IsLoading = false;
    }

    void WebView_Navigating(object sender, WebNavigatingEventArgs e)
    {
#if ANDROID
        if(IsLoading is false)
            IsLoading = true;
#else
        IsLoading = false;
#endif
    }

    async void OnBackButtonClicked(object sender, EventArgs e)
    {
        if (LocaleyesLatestNewsWebView.CanGoBack)
        {
            LocaleyesLatestNewsWebView.GoBack();
        }
        else
        {
            await Navigation.PopAsync();
        }
    }

    void OnForwardButtonClicked(object sender, EventArgs e)
    {
        if (LocaleyesLatestNewsWebView.CanGoForward)
        {
            LocaleyesLatestNewsWebView.GoForward();
        }
    }

    void RefreshWebView()
    {
        if (IsBusy)
            return;

        LocaleyesLatestNewsWebView.Reload();

        IsBusy = false;
        IsRefreshing = false;
    }

    void OnHomeNavigationButton_Clicked(object sender, EventArgs e)
    {
        LocaleyesLatestNewsWebView.Source = "https://localeyes.dk/category/agency/";
    }
}