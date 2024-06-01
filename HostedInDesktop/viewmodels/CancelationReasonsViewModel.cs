using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HostedInDesktop.Abstract;
using HostedInDesktop.Data.Models;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Messages;
using HostedInDesktop.Utils;
using HostedInDesktop.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HostedInDesktop.viewmodels
{
    public partial class CancelationReasonsViewModel : ObservableObject
    {
        ICancellationService _cancellationService = new CancellationService();

        [ObservableProperty]
        private string selectedReason;

        [ObservableProperty]
        private Booking _booking;

        ISharedService _sharedService;
        public CancelationReasonsViewModel(ISharedService sharedService)
        {
            _sharedService = sharedService;
            WeakReferenceMessenger.Default.Register<BookingToCancelMessage>(this, (r, m) =>
            {
                Booking = m.Value;
            });
            GetSelectedBooking();
        }

        [RelayCommand]
        private async Task OnSubmit()
        {
            try
            {
                if (SelectedReason != null || string.IsNullOrWhiteSpace(SelectedReason))
                {
                    Cancellation cancellation = new Cancellation()
                    {
                        cancellationDate = DateTime.Now,
                        cancellator = App.user,
                        reason = SelectedReason!,
                        booking = Booking
                    };
                    Cancellation created = await _cancellationService.cancelBooking(cancellation);
                    if (created != null)
                    {
                        _sharedService.Add<Cancellation>("CreatedCancellation", created);
                        WeakReferenceMessenger.Default.Send(new CancellationCreatedMessage(created));
                        await Shell.Current.GoToAsync(nameof(CancellationConfirmationView));
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Motivo requerido", "Por favor, selecciona un motivo para continuar", "Ok");
                }
            }
            catch (UnauthorizedAccessException)
            {
                await Shell.Current.DisplayAlert("La sesión caducó", "La sesión caducó debido a inactividad.", "Ir a inicio de sesión");
                await Shell.Current.GoToAsync("///Login");
            }
            catch (ApiException ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
                return;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "ok");
            }
        }

        private void GetSelectedBooking()
        {
            try
            {
                Booking = _sharedService.GetValue<Booking>("BookingDetail");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [RelayCommand]
        public async Task GoBack()
        {
            if (App.hostMode)
            {
                await Shell.Current.GoToAsync(nameof(BookingDetailsView));
            }
            else
            {
                await Shell.Current.GoToAsync(nameof(BookingDetailsView));
            }
        }
    }
}
