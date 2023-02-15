using CommunityToolkit.Maui.Alerts;
using LocalEyesTipApp.DataServices;
using LocalEyesTipApp.Interfaces;
using LocalEyesTipApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Storage;
using System.IO;
using System.IO.Pipelines;

namespace LocalEyesTipApp.Pages;

[QueryProperty(nameof(Message), nameof(Message))]
public partial class SendTipPage : ContentPage
{
    private readonly IRestDataService _restDataService;
    private MessageModel _message;


    public SendTipPage(IRestDataService dataService)
    {
        InitializeComponent();

        _restDataService = dataService;

        BindingContext = this;
    }

    public MessageModel Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged();
        }
    }

    private async void OnSendBtnClicked(object sender, EventArgs e)
    {
        var userInputIsValid = await ValidateInputs();

        if (!userInputIsValid)
            return;

        DisableButtonsAndEntries();

        // Set the activity indicator and make it visible...
        ActivityIndicator indicator = new()
        {
            IsRunning = true,
            IsVisible = true,
            Color = Colors.Grey,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            HeightRequest = 200,
            WidthRequest = 200
        };
        mainView.Children.Add(indicator);

        // Send message and files to api server.
        var result = await _restDataService.SendTipAsync(Message);

        // Stop the activity indicator again and make it invisible.
        indicator.IsRunning = false;
        indicator.IsVisible = false;

        EnableButtonsAndEntries();

        // Check the result and display alert.
        if (result.Succeded)
        {
            await DisplayAlert("Beskeden blev sendt", "Tak for din henvendelse. Beskeden er nu sendt til vores fotografer.", "Ok");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            bool answer = await DisplayAlert("Der opstod en uventet fejl!", result.Message, "Prøv igen?", "Afbryd");

            if (answer)
                return;

            await Shell.Current.GoToAsync("..");
        }
    }

    private async void OnAddFilesButton_Clicked(object sender, EventArgs e)
    {
        Message.MediaFiles = await FilePicker.PickMultipleAsync(new PickOptions
        {
            PickerTitle = "Vælg billede eller video",
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.iOS, new[] { "public.image", "public.video" } },
                { DevicePlatform.Android, new[] { "image/jpeg", "video/mp4",  } }
            })
        });

        List<string> files = new();

        foreach (var file in Message.MediaFiles)
        {
            files.Add(file.FileName);
        }

        filesListView.ItemsSource = files;
    }

    private void DisableButtonsAndEntries()
    {
        // Disable buttons and entries to prevent dubble posting issues.
        addFilesButton.IsEnabled = false;
        sendTipButton.IsEnabled = false;
        tipMessageEntry.IsEnabled = false;
        addressEntry.IsEnabled = false;
        phoneNumberEntry.IsEnabled = false;
        mailEntry.IsEnabled = false;
    }

    private void EnableButtonsAndEntries()
    {
        // Activate buttons and entries again.
        addFilesButton.IsEnabled = true;
        sendTipButton.IsEnabled = true;
        tipMessageEntry.IsEnabled = true;
        addressEntry.IsEnabled = true;
        phoneNumberEntry.IsEnabled = true;
        mailEntry.IsEnabled = true;
    }

    private async Task<bool> ValidateInputs()
    {
        if (string.IsNullOrEmpty(tipMessageEntry.Text))
        {
            await DisplayAlert("Påkrævet!", "Tip skal udfyldes! Prøv venligst igen.", "Ok");
            return false;
        }

        if (string.IsNullOrEmpty(addressEntry.Text))
        {
            await DisplayAlert("Påkrævet!", "Du skal angive en adresse! Prøv venligst igen.", "Ok");
            return false;
        }

        return true;
    }
}