﻿using Application.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CurrentTime : ICurrentTime
    {
        public DateTime GetCurrentTime()
        {
           return DateTime.Now;
        }
    }
}
