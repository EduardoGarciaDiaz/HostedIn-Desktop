using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Enums
{
    public enum AccommodationServices
    {
        internet,
        tv,
        kitchen,
        washing_machine,
        parking,
        air_conditioning,
        pool,
        garden,
        light,
        water
    }

    public static class AccommodationServiceDescriptions
    {
        public static string GetDescriptionForService(string service)
        {
            switch (service)
            {
                case "internet":
                    return "Internet";
                case "tv":
                    return "TV";
                case "kitchen":
                    return "Cocina";
                case "washing machine":
                    return "Lavadora";
                case "parking":
                    return "Estacionamiento";
                case "air conditioning":
                    return "Aire acondicionado";
                case "pool":
                    return "Alberca";
                case "garden":
                    return "Jardín";
                case "light":
                    return "Luz";
                case "water":
                    return "Agua";
                default:
                    return service;
            }
        }
    }
}
