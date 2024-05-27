using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HostedInDesktop.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HostedInDesktop.viewmodels.ModelObservable
{
    public partial class AccommodationBasic : ObservableObject
    {
        [ObservableProperty]
        private string _basicText;

        [ObservableProperty]
        private int _value;
        public ICommand IncreaseCommand { get; }
        public ICommand DecreaseCommand { get; }

        public AccommodationBasic()
        {
            IncreaseCommand = new RelayCommand(Increase);
            DecreaseCommand = new RelayCommand(Decrease);
        }

        private void Increase()
        {
            Value++;
        }

        private void Decrease()
        {
            if (Value > 1)
                Value--;
        }
    }
}
