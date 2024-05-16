using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedIn_DesktopClient.Models
{
    public class User
    {
        public string email { get; set; }
        public string fullName { get; set; }
        public string birthDate { get; set; }
        public string phoneNumber { get; set; }
        public string password { get; set; }
        public string occupation { get; set; }
        public string residence { get; set; }
        public byte[] profilePhoto { get; set; }

        public User(string email, string fullName, DateTime birthDate, string phoneNumber, string password, string occupation, string residence, byte[] profilePhoto)
        {
            this.email = email;
            this.fullName = fullName;
            this.birthDate = birthDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            this.phoneNumber = phoneNumber;
            this.password = password;
            this.occupation = occupation;
            this.residence = residence;
            this.profilePhoto = profilePhoto;
        }
    }

}
