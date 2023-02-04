using LocalEyesTipApp.Interfaces;
using LocalEyesTipApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Maui.Storage;
using System.IO;
using System.IO.Pipelines;

namespace LocalEyesTipApp.Pages;

[QueryProperty(nameof(MessageModel), "MessageModel")]
public partial class SendTipPage : ContentPage
{
    private readonly IRestDataService _restDataService;
    
    MessageModel _message;

    public MessageModel MessageModel
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged();
        }
    }
    
    public SendTipPage(IRestDataService dataService)
    {
        InitializeComponent();

        _restDataService = dataService;

        BindingContext = this;
    }


    private async void OnSendBtnClicked(object sender, EventArgs e)
    {
        var result = await _restDataService.SendTipAsync(MessageModel);

        if (result.Succeded)
        {
            await DisplayAlert("Beskeden blev sendt", "Tak for din henvendelse. Beskeden er nu sendt til vores fotografer.", "Ok");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            bool answer = await DisplayAlert("Der opstod en uventet fejl!", result.Message, "Prøv igen?", "Afbryd");

            if (answer)
            {
                var navigationParameter = new Dictionary<string, object>
                {
                    { nameof(MessageModel), new MessageModel() }
                };

                await Shell.Current.GoToAsync(nameof(SendTipPage), navigationParameter);
            }

            await Shell.Current.GoToAsync("..");
        }
    }

    private async void OnAddFilesButton_Clicked(object sender, EventArgs e)
    {
        MessageModel.MediaFiles = await FilePicker.PickMultipleAsync(new PickOptions
        {
            PickerTitle = "Vælg billede eller video",
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.iOS, new[] { "public.image", "public.video" } },
                { DevicePlatform.Android, new[] { "image/jpeg", "video/mp4",  } }
            })
        });

        List<string> files = new();

        foreach (var file in MessageModel.MediaFiles)
        {
            files.Add(file.FileName);
        }

        filesListView.ItemsSource = files;
    }
}