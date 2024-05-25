using CommunityToolkit.Mvvm.ComponentModel;
using HostedInDesktop.Data.JsonConverters;
using Newtonsoft.Json;
using System;

namespace HostedInDesktop.Data.Models
{
    [ObservableObject]
    public partial class User
    {
        public string _id {  get; set; }
        public string email { get; set; }  
        public string password { get; set; }
        public string fullName { get; set; }
        public string birthDate { get; set; }
        public string phoneNumber { get; set; }
        public string occupation { get; set; }
        public string residence { get; set; }
        public ProfilePhoto profilePhoto { get; set; }
        public string[] roles { get; set; }
    }
}
