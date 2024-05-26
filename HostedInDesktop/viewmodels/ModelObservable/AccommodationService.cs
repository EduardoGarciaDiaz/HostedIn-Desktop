using CommunityToolkit.Mvvm.ComponentModel;
using HostedInDesktop.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels.ModelObservable
{
    public class AccommodationService : ObservableObject
    {
        public string ServiceName { get; set; }
        public AccommodationServices ServiceEnum { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}
