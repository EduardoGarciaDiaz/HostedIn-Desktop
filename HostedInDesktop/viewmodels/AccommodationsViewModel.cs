using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels
{
    public partial class AccommodationsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Accommodation> accommodations;

        public AccommodationsViewModel()
        {
          
        }

        [RelayCommand]
        private void SelectAccommodation(Accommodation accommodation)
        {
            // Lógica para manejar la selección de un alojamiento
        }
    }
}
