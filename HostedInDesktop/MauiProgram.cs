using HostedInDesktop.viewmodels;
using HostedInDesktop.Views;
using Microsoft.Extensions.Logging;

namespace HostedInDesktop
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<Login>();
            builder.Services.AddSingleton<EditProfileViewModel>();
            builder.Services.AddSingleton<EditProfile>();
            builder.Services.AddSingleton<DeleteAccountViewModel>();
            builder.Services.AddSingleton<DeleteAccount>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
