using HostedInDesktop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Utils;

public static class TranslatorToSpanish
{
    public static String TranslateBookingStatusValue(String bookingStatus)
    {
        if (bookingStatus.Equals(BookingStatus.CURRENT.GetDescription()))
        {
            return "Vigente";
        }
        else if (bookingStatus.Equals(BookingStatus.OVERDUE.GetDescription()))
        {
            return "Vencida";
        }else
        {
            return "Cancelada";
        }

    }
}
