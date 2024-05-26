using Grpc;
using Grpc.Net.Client;
using HostedInDesktop.Data.Services;
using HostedInDesktop.viewmodels;
using HostedInDesktop.Views;
using Microsoft.Extensions.Logging;

namespace HostedInDesktop
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            string BaseAddres = "http://localhost:3002";
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
            builder.Services.AddSingleton<ProfileViewModel>();
            builder.Services.AddSingleton<Profile>();

/*            builder.Services.AddScoped(services =>
            {
                var channel = GrpcChannel.ForAddress(BaseAddres);
                return new MultimediaService.MultimediaServiceClient(channel);
            });

            builder.Services.AddScoped(services =>
            {
                var channel = GrpcChannel.ForAddress(BaseAddres);
                return new StaticticsService.StaticticsServiceClient(channel);
            });


            builder.Services.AddScoped<MultimediaServiceImpl>();*/
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
