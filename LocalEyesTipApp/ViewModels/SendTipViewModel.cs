using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LocalEyesTipApp.DataServices;
using LocalEyesTipApp.Interfaces;
using LocalEyesTipApp.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace LocalEyesTipApp.ViewModels;

[QueryProperty(nameof(Message), nameof(Message))]
public partial class SendTipViewModel : BaseViewModel
{
    private readonly IRestDataService _restDataService;

    public SendTipViewModel(IRestDataService dataService)
    {
        _restDataService = dataService;

        Message.MediaFiles ??= new();
    }

    public ObservableCollection<string> Files { get; private set; } = new();

    [ObservableProperty]
    bool addFilesButtonEnable;

    [ObservableProperty]
    bool addPhotoButtonEnable;

    [ObservableProperty]
    bool addVideoButtonEnable;

    [ObservableProperty]
    bool sendTipButtonEnable;

    [ObservableProperty]
    bool tipMessageEntryEnable;

    [ObservableProperty]
    bool addressEntryEnable;

    [ObservableProperty]
    bool phoneNumberEntryEnable;

    [ObservableProperty]
    bool mailEntryEnable;

    [ObservableProperty]
    MessageModel message;

    [RelayCommand]
    async void SendTipAsync()
    {
        var userInputIsValid = await ValidateInputs();

        if (!userInputIsValid)
            return;

        DisableButtonsAndEntries();

        IsBusy = true;

        // Send message and files to api server.
        var result = await _restDataService.SendTipAsync(Message);

        EnableButtonsAndEntries();

        IsBusy = false;

        // Check the result and display alert.
        if (result.Succeded)
        {
            await Shell.Current.DisplayAlert("Beskeden blev sendt", "Tak for din henvendelse. Beskeden er nu sendt til vores fotografer.", "Ok");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            bool answer = await Shell.Current.DisplayAlert("Der opstod en uventet fejl!", result.Message, "Prøv igen?", "Afbryd");

            if (answer)
                return;

            await Shell.Current.GoToAsync("..");
        }
    }

    [RelayCommand]
    async void AddFilesAsync()
    {
        PickOptions options = new()
        {
            PickerTitle = "Vælg billeder/videoer"
        };

        try
        {
            var result = await FilePicker.PickMultipleAsync(options);

            if (result is null)
                return;

            foreach (var file in result)
            {
                Message.MediaFiles.Add(file);
                Files.Add(file.FileName);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Der opstod en uventet fejl: ", ex.Message);
            await Shell.Current.DisplayAlert("Der opstod en uventet fejl!", "Forsøg venligst igen, eller kontakt udvikleren hvis problemet fortsætter!", "Ok");
            return;
        }
    }

    [RelayCommand]
    async void AddPhotoAsync()
    {
        try
        {
            MediaPickerOptions options = new()
            {
                Title = "Vælg Billede"
            };

            var result = await MediaPicker.PickPhotoAsync(options);

            if (result is null)
                return;

            Message.MediaFiles.Add(result);
            Files.Add(result.FileName);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Der opstod en uventet fejl: ", ex.Message);
            await Shell.Current.DisplayAlert("Der opstod en uventet fejl!", "Forsøg venligst igen, eller kontakt udvikleren hvis problemet fortsætter!", "Ok");
            return;
        }
    }

    [RelayCommand]
    async void AddVideoAsync()
    {
        try
        {
            MediaPickerOptions options = new()
            {
                Title = "Vælg Video"
            };

            var result = await MediaPicker.PickVideoAsync(options);
        
            if (result == null)
                return;
        
            Message.MediaFiles.Add(result);
            Files.Add(result.FileName);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Der opstod en uventet fejl: ", ex.Message);
            await Shell.Current.DisplayAlert("Der opstod en uventet fejl!", "Forsøg venligst igen, eller kontakt udvikleren hvis problemet fortsætter!", "Ok");
            return;
        }
    }

    void DisableButtonsAndEntries()
    {
        // Disable buttons and entries to prevent dubble posting issues.
        AddFilesButtonEnable = false;
        AddPhotoButtonEnable = false;
        AddVideoButtonEnable = false;
        SendTipButtonEnable = false;
        TipMessageEntryEnable = false;
        AddressEntryEnable = false;
        PhoneNumberEntryEnable = false;
        MailEntryEnable = false;
    }

    void EnableButtonsAndEntries()
    {
        // Activate buttons and entries again.
        AddFilesButtonEnable = true;
        AddPhotoButtonEnable = true;
        AddVideoButtonEnable = true;
        SendTipButtonEnable = true;
        TipMessageEntryEnable = true;
        AddressEntryEnable = true;
        PhoneNumberEntryEnable = true;
        MailEntryEnable = true;
    }

    async Task<bool> ValidateInputs()
    {
        if (string.IsNullOrEmpty(Message.MessageText))
        {
            await Shell.Current.DisplayAlert("Påkrævet!", "Tip skal udfyldes! Prøv venligst igen.", "Ok");
            return false;
        }

        if (string.IsNullOrEmpty(Message.Address))
        {
            await Shell.Current.DisplayAlert("Påkrævet!", "Du skal angive en adresse! Prøv venligst igen.", "Ok");
            return false;
        }

        return true;
    }

    [RelayCommand]
    async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}