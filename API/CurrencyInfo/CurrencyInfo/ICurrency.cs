﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CurrencyInfo {
    public class ICurrency {
        public string Code { get; set; }
        public string Description { get; set; }
        public double ExchangeRate { get; set; }
    }
}