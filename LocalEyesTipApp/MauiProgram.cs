using LocalEyesTipApp.DataServices;
using LocalEyesTipApp.Interfaces;
using LocalEyesTipApp.Pages;
using CommunityToolkit.Maui;

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
                fonts.AddFont("Fa-Brands-Regular-400.otf", "FontBrandsRegular");
                fonts.AddFont("Fa-Regular-400.otf", "FontRegular");
                fonts.AddFont("Fa-Solid-900.otf", "FontSolid");
            }).UseMauiCommunityToolkit();


            builder.Services.AddSingleton<IRestDataService, RestDataService>();
            builder.Services.AddSingleton<LatestNewsPage>();
            builder.Services.AddSingleton<AboutPage>();
            builder.Services.AddTransient<SendTipPage>();
            return builder.Build();
        }
    }
}