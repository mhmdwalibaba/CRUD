﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class CountryResposnse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }

    }
}
