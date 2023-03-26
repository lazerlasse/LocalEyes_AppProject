using LocalEyesTipApp.DataServices;
using LocalEyesTipApp.Interfaces;
using LocalEyesTipApp.Pages;
using CommunityToolkit.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Hosting;
using LocalEyesTipApp.ViewModels;

namespace LocalEyesTipApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkit();

            // Add Services...
            builder.Services.AddSingleton<IRestDataService, RestDataService>();

            // Add View Models...
            builder.Services.AddTransient<SendTipViewModel>();

            // Add Pages...
            builder.Services.AddSingleton<LatestNewsPage>();
            builder.Services.AddSingleton<AboutPage>();
            builder.Services.AddTransient<SendTipPage>();
            
            
            return builder.Build();
        }
    }
}