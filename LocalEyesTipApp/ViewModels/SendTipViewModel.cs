using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
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
    }

    public ObservableCollection<string> Files { get; private set; } = new();

    [ObservableProperty]
    MessageModel message;

    [RelayCommand]
    async void SendTipAsync()
    {
        var userInputIsValid = await ValidateInputs();

        if (!userInputIsValid)
            return;

        IsBusy = true;

        // Send message and files to api server.
        var result = await _restDataService.SendTipAsync(Message);

        IsBusy = false;

        // Check the result and display alert.
        if (result.Succeded)
        {
            await Shell.Current.DisplayAlert("Beskeden blev sendt", "Tak for din henvendelse. Beskeden er nu sendt til vores fotografer.", "Ok");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            bool answer = await Shell.Current.DisplayAlert("Der opstod en uventet fejl!", result.Message, "Pr�v igen?", "Afbryd");

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
            PickerTitle = "V�lg billeder/videoer"
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
            await Shell.Current.DisplayAlert("Der opstod en uventet fejl!", "Fors�g venligst igen, eller kontakt udvikleren hvis problemet forts�tter!", "Ok");
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
                Title = "V�lg Billede"
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
            await Shell.Current.DisplayAlert("Der opstod en uventet fejl!", "Fors�g venligst igen, eller kontakt udvikleren hvis problemet forts�tter!", "Ok");
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
                Title = "V�lg Video"
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
            await Shell.Current.DisplayAlert("Der opstod en uventet fejl!", "Fors�g venligst igen, eller kontakt udvikleren hvis problemet forts�tter!", "Ok");
            return;
        }
    }

    async Task<bool> ValidateInputs()
    {
        if (string.IsNullOrEmpty(Message.MessageText))
        {
            await Shell.Current.DisplayAlert("P�kr�vet!", "Tip skal udfyldes! Pr�v venligst igen.", "Ok");
            return false;
        }

        if (string.IsNullOrEmpty(Message.Address))
        {
            await Shell.Current.DisplayAlert("P�kr�vet!", "Du skal angive en adresse! Pr�v venligst igen.", "Ok");
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