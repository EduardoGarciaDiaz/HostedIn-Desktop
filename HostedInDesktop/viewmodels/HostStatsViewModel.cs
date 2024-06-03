using CommunityToolkit.Mvvm.ComponentModel;
using HostedInDesktop.Data.GrpcModels;
using HostedInDesktop.Data.Services;
using HostedInDesktop.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels
{
    public partial class HostStatsViewModel : ObservableObject
    {

        public ObservableCollection<AccommodationGrpc> MostBookedAccommodations { get; } = new ObservableCollection<AccommodationGrpc>();
        public ObservableCollection<MonthEarning> MonthEarnings { get; } = new ObservableCollection<MonthEarning>();
        private StaticticsServiceImpl _staticticsService = new StaticticsServiceImpl();


        public HostStatsViewModel()
        {
            LoadMostBookedHostAccommodations();
            LoadHostMonthEarnings();
        }

        public async Task LoadMostBookedHostAccommodations()
        {
            try
            {
                var accommodations = await _staticticsService.GetMostHostBookedAccommodations(App.user._id);
                MostBookedAccommodations.Clear();
                foreach (var accommodation in accommodations)
                {
                    MostBookedAccommodations.Add(accommodation);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error ", GenericExceptionMessage.GetDescription(ExceptionMessages.GENERIC_DESKTOP_EXCEPTION_MEESAGE), "Ok");
                return;
            }
        }

        public async Task LoadHostMonthEarnings()
        {
            try
            {
                var earnings = await _staticticsService.GetHostEarnings(App.user._id);
                MonthEarnings.Clear();
                foreach (var earning in earnings)
                {
                    MonthEarnings.Add(earning);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error ", GenericExceptionMessage.GetDescription(ExceptionMessages.GENERIC_DESKTOP_EXCEPTION_MEESAGE), "Ok");
                return;
            }
        }
    }
}
