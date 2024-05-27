using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using HostedInDesktop.Data.GrpcModels;
using HostedInDesktop.Data.Services;

namespace HostedInDesktop.viewmodels
{
    public partial class GuestStatsViewModel : ObservableObject
    {
        public ObservableCollection<AccommodationGrpc> MostBookedAccommodations { get; } = new ObservableCollection<AccommodationGrpc>();
        public ObservableCollection<AccommodationGrpc> BestRatedAccommodations { get; } = new ObservableCollection<AccommodationGrpc>();
        private StaticticsServiceImpl _staticticsService = new StaticticsServiceImpl();


        public GuestStatsViewModel() { 
            LoadBestRatedAccommodations();
            LoadMostBookedAccommodations();
        }

        public async Task LoadMostBookedAccommodations()
        {
            try
            {
                var accommodations = await _staticticsService.GetMostBookedAccommodations();
                MostBookedAccommodations.Clear();
                foreach(var accommodation in accommodations)
                {
                    MostBookedAccommodations.Add(accommodation);
                } 
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error ", ex.Message, "Ok");
                return;
            }
        }

        public async Task LoadBestRatedAccommodations()
        {
            try
            {
                var accommodations = await _staticticsService.GetBestRatedAccommodations();
                BestRatedAccommodations.Clear();
                foreach (var accommodation in accommodations)
                {
                    BestRatedAccommodations.Add(accommodation);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error ", ex.Message, "Ok");
                return;
            }
        }


    }
}
