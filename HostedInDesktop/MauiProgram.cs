using HostedInDesktop.Reusable;
using CommunityToolkit.Maui;
using HostedInDesktop.Utils;
using Grpc;
using Grpc.Net.Client;
using HostedInDesktop.Data.Services;
using HostedInDesktop.viewmodels;
using HostedInDesktop.Views;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Syncfusion.Maui.Core.Hosting;
using HostedInDesktop.Abstract;

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
                .ConfigureSyncfusionCore()
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

            builder.Services.AddSingleton<BookingsView>();
            builder.Services.AddSingleton<BookingDetailsView>();

            builder.Services.AddTransient<BookingsGuestViewViewModel>();
            builder.Services.AddTransient<BookingDetailsViewModel>();

            builder.Services.AddTransient<BookingDetailsViewModel>();
            builder.Services.AddTransient<CancelationReasonsViewModel>();
            builder.Services.AddTransient<CancellationConfirmationViewModel>();

            builder.Services.AddTransient<GuestViewViewModel>();
            builder.Services.AddTransient<HostViewViewModel>();

            builder.Services.AddSingleton<ISharedService, SharedService>();
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
