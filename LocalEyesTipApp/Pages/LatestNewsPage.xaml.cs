using LocalEyesTipApp.Models;
using System.Windows.Input;

namespace LocalEyesTipApp.Pages;

public partial class LatestNewsPage : ContentPage
{
    private bool isRefreshing;

    public Command RefreshCommand { get; }

    public LatestNewsPage()
    {
        InitializeComponent();
        RefreshCommand = new Command(RefreshWebView);
        LoadLocalEyesWebSite();
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

    void WebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
        if(labelLoading.IsVisible is true)
            labelLoading.IsVisible = false;
    }

    void WebView_Navigating(object sender, WebNavigatingEventArgs e)
    {
        if(labelLoading.IsVisible is false)
            labelLoading.IsVisible = true;
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
        LoadLocalEyesWebSite();
    }

    async void LoadLocalEyesWebSite()
    {
        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
        {
            LocaleyesLatestNewsWebView.Source = "https://localeyes.dk/category/agency/";
        }
        else
        {
            await Shell.Current.DisplayAlert("Ingen internet!", "Du har muligvis ikke forbindelse til internettet og nyhederne kan derfor ikke indlæses. Opret forbindelse til internettet og prøv igen.", "Ok");
        }
    }
}