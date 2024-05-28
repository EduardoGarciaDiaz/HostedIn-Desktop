﻿using CommunityToolkit.Mvvm.Messaging.Messages;
using HostedInDesktop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Messages
{
    public class BookingSelectedMessage : ValueChangedMessage<Booking>
    {
        public BookingSelectedMessage(Booking value) : base(value)
        {
        }
    }
}
