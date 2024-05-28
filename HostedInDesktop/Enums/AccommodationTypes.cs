using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Enums
{
    public enum AccommodationTypes
    {
        house,
        apartment,
        cabin,
        camp,
        camper,
        ship
    }

    public static class AccommodationTypeDescriptions
    {
        public static string GetDescriptionForType(string type)
        {
            return type switch
            {
                "house" => "Casa",
                "apartement" => "Departamento",
                "cabin" => "Cabaña",
                "camp" => "Campamento",
                "camper" => "Casa rodante",
                "ship" => "Barco",
                _ => type,
            };
        }
    }
}