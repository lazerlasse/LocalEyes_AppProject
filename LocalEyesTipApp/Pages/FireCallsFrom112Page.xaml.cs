namespace LocalEyesTipApp.Pages;

public partial class FireCallsFrom112Page : ContentPage
{
    private bool isRefreshing;
    public Command RefreshCommand { get; }
    
    public FireCallsFrom112Page()
	{
		InitializeComponent();
        RefreshCommand = new Command(RefreshWebView);
        Load112PulsWebSite();
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
    
    void RefreshWebView()
    {
        if (IsBusy)
            return;

        FireCallsFrom112WebView.Reload();

        IsBusy = false;
        IsRefreshing = false;
    }
    
    async void Load112PulsWebSite()
    {
        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
        {
            FireCallsFrom112WebView.Source = "http://odin.dk/112puls/";
        }
        else
        {
            await Shell.Current.DisplayAlert("Ingen internet!", "Du har muligvis ikke forbindelse til internettet og nyhederne kan derfor ikke indlæses. Opret forbindelse til internettet og prøv igen.", "Ok");
        }
    }
}