using LocalEyesTipApp.DataServices;
using LocalEyesTipApp.Interfaces;
using LocalEyesTipApp.Pages;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Hosting;
using LocalEyesTipApp.ViewModels;
using CommunityToolkit.Maui;

namespace LocalEyesTipApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                // Initialize the .NET MAUI Community Toolkit MediaElement by adding the below line of code
                .UseMauiCommunityToolkitMediaElement()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Add Services...
            builder.Services.AddSingleton<IRestDataService, RestDataService>();

            // Add View Models...
            builder.Services.AddTransient<SendTipViewModel>();
            builder.Services.AddSingleton<SettingsViewModel>();

            // Add Pages...
            builder.Services.AddSingleton<LatestNewsPage>();
            builder.Services.AddTransient<SendTipPage>();
            builder.Services.AddSingleton<AboutPage>();
            builder.Services.AddSingleton<FireCallsFrom112Page>();
            builder.Services.AddSingleton<SettingsPage>();

            return builder.Build();
        }
    }
}