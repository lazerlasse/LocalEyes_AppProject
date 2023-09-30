using LocalEyesTipApp.Models;
using Microsoft.Maui.Controls;
using LocalEyesTipApp.DataServices;
using LocalEyesTipApp.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
using LocalEyesTipApp.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LocalEyesTipApp.Pages;

namespace LocalEyesTipApp.ViewModels;


public partial class SendTipViewModel : BaseViewModel
{
    private readonly IRestDataService _restDataService;

    public SendTipViewModel(IRestDataService dataService)
    {
        Message = new();

        _restDataService = dataService;
    }

    public ObservableCollection<string> Files { get; private set; } = new();
    public ObservableCollection<MediaToUpload> MediaToUploads { get; private set; } = new();

    [ObservableProperty]
    MessageModel message;

    [RelayCommand]
    async void SendTipAsync()
    {
        var userInputIsValid = await ValidateInputs();

        if (!userInputIsValid)
            return;

        await Shell.Current.DisplayAlert("Send tip:", "Du er ved at sende dit tip til os. Bemærk dog at hvis du har vedhæftet filer, så kan det tage noget tid at sende dit tip! Hav derfor tolmodighed og forlad ikke appen før overførslen er fuldført! Tryk ok for at fortsætte...", "OK");

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
                MediaToUploads.Add(new MediaToUpload { FileName = file.FileName, ImageFilePath = file.FullPath });
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
            MediaToUploads.Add(new MediaToUpload { FileName = result.FileName, ImageFilePath = result.FullPath });
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

            if (result is null)
                return;

            Message.MediaFiles.Add(result);
            Files.Add(result.FileName);
            MediaToUploads.Add(new MediaToUpload { FileName = result.FileName, ImageFilePath = result.FullPath });
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Der opstod en uventet fejl: ", ex.Message);
            await Shell.Current.DisplayAlert("Der opstod en uventet fejl!", "Forsøg venligst igen, eller kontakt udvikleren hvis problemet fortsætter!", "Ok");
            return;
        }
    }

    [RelayCommand]
    async void TakePhotoAsync()
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            try
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo is null)
                    return;

                Message.MediaFiles.Add(photo);
                Files.Add(photo.FileName);
                MediaToUploads.Add(new MediaToUpload { FileName = photo.FileName, ImageFilePath = photo.FullPath });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Der opstod en uventet fejl: ", ex.Message);
                await Shell.Current.DisplayAlert("Der opstod en uventet fejl!", "Forsøg venligst igen, eller kontakt udvikleren hvis problemet fortsætter!", "Ok");
                return;
            }
        }
    }

    [RelayCommand]
    async void RecordVideoAsync()
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            try
            {
                FileResult video = await MediaPicker.Default.CaptureVideoAsync();

                if (video is null)
                    return;

                Message.MediaFiles.Add(video);
                Files.Add(video.FileName);
                MediaToUploads.Add(new MediaToUpload { FileName = video.FileName, ImageFilePath = video.FullPath });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Der opstod en uventet fejl: ", ex.Message);
                await Shell.Current.DisplayAlert("Der opstod en uventet fejl!", "Forsøg venligst igen, eller kontakt udvikleren hvis problemet fortsætter!", "Ok");
                return;
            }
        }
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
    void RemoveMediaFileAsync(string fileName)
    {
        var mediaUploadToDelete = MediaToUploads.FirstOrDefault(f => f.FileName == fileName);
        if (mediaUploadToDelete is not null)
        {
            MediaToUploads.Remove(mediaUploadToDelete);
        }

        var fileToRemoveFromTip = Message.MediaFiles.FirstOrDefault(f => f.FileName == fileName);
        if (fileToRemoveFromTip is not null)
        {
            Message.MediaFiles.Remove(fileToRemoveFromTip);
        }
    }
}