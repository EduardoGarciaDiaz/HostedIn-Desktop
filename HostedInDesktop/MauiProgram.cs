using CommunityToolkit.Maui;
using HostedInDesktop.Utils;
using HostedInDesktop.viewmodels;
using HostedInDesktop.Views;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace HostedInDesktop
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp(true)
                .UseMauiCommunityToolkitMediaElement()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IMapService, MapService>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<Login>();
            builder.Services.AddSingleton<EditProfileViewModel>();
            builder.Services.AddSingleton<EditProfile>();
            builder.Services.AddSingleton<DeleteAccountViewModel>();
            builder.Services.AddSingleton<DeleteAccount>();
            builder.Services.AddSingleton<ProfileViewModel>();
            builder.Services.AddSingleton<Profile>();
            builder.Services.AddSingleton<AccommodationDetailsViewModel>();
            builder.Services.AddSingleton<AccommodationDetails>();
            builder.Services.AddSingleton<AccommodationFormViewModel>();
            builder.Services.AddSingleton<AccommodationForm>(); 
            builder.Services.AddSingleton<AccommodationFormType>();
            builder.Services.AddSingleton<AccommodationFormLocation>();
            builder.Services.AddSingleton<AccommodationFormBasics>();
            builder.Services.AddSingleton<AccommodationFormServices>();
            builder.Services.AddSingleton<AccommodationFormMultimedia>();
            builder.Services.AddSingleton<AccommodationFormInformation>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
