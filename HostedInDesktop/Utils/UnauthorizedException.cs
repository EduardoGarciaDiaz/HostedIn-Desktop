﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Utils
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) 
        { 
        }

    }
}
