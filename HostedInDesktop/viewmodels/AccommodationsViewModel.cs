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
            Accommodations = new ObservableCollection<Accommodation>
            {
                new Accommodation { Name = "Hotel A", Location = "City A", ImageUrl = "https://via.placeholder.com/150" },
                new Accommodation { Name = "Hotel B", Location = "City B", ImageUrl = "https://via.placeholder.com/150" },
                 new Accommodation { Name = "Hotel A", Location = "City A", ImageUrl = "https://via.placeholder.com/150" },
                new Accommodation { Name = "Hotel B", Location = "City B", ImageUrl = "https://via.placeholder.com/150" },
                 new Accommodation { Name = "Hotel A", Location = "City A", ImageUrl = "https://via.placeholder.com/150" },
                new Accommodation { Name = "Hotel B", Location = "City B", ImageUrl = "https://via.placeholder.com/150" },
                // Añade más objetos de prueba
            };
        }

        [RelayCommand]
        private void SelectAccommodation(Accommodation accommodation)
        {
            // Lógica para manejar la selección de un alojamiento
        }
    }
}
