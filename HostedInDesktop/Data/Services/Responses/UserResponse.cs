﻿using HostedInDesktop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Data.Services.Responses
{
    public class UserResponse
    {
        public string Message { get; set; }
        public User User { get; set; }
    }
}
