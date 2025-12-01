using ExchangeRate_MAUIApp.Services;
using ExchangeRate_MAUIApp.ViewModels;
using ExchangeRate_MAUIApp.Views;
using Microcharts.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace ExchangeRate_MAUIApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

       
            builder.Services.AddHttpClient<IExchangeRateService, ExchangeRateService>(client =>
            {
                client.BaseAddress = new Uri("https://www.nbrm.mk/");
            });

   
            builder.Services.AddTransient<ExchangeRateViewModel>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<ChartPage>();
#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();

        }
    }
}
