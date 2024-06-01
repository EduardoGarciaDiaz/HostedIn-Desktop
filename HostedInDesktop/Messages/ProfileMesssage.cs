﻿using CommunityToolkit.Mvvm.Messaging.Messages;
using HostedInDesktop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Messages
{
    public class ProfileMesssage : ValueChangedMessage<User>
    {
        public ProfileMesssage(User value) : base(value)
        {
        }
    }
}
