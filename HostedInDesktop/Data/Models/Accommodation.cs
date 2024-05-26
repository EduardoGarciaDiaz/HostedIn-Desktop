using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Models
{
    public class Accommodation
    {
        public string _id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string rules { get; set; }
        public string accommodationType { get; set; }
        public double nightPrice { get; set; }
        public int guestsNumber { get; set; }
        public int roomsNumber { get; set; }
        public int bedsNumber { get; set; }
        public int bathroomsNumber { get; set; }
        public string[] accommodationServices { get; set; }
        public Location location { get; set; }
        public User user { get; set; }
        private byte[] _mainImage;

        public byte[] mainImage
        {
            get => _mainImage;
            set
            {
                _mainImage = value;
                OnPropertyChanged(nameof(mainImage));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

public class Location
{
    public string _id { get; set; }
    public string type { get; set; }
    public double[] coordinates { get; set; }
    public double latitude { get; set; }
    public double longitude { get; set; }
    public string address { get; set; }
}


