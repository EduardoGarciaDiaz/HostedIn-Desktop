using CommunityToolkit.Mvvm.ComponentModel;
using HostedInDesktop.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels.ModelObservable
{
    [ObservableObject]
    public partial class AccommodationType
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public AccommodationTypes BasicEnum { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}
