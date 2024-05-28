using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.viewmodels.ModelObservable
{
    [ObservableObject]
    public partial class UserReview
    {
        public string GuestName { get; set; }
        public float ValueRating { get; set; }
        public string Description { get; set; }
        public ImageSource ProfilePhoto { get; set; }
    }
}
