using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entits;

namespace ServiceContracts.DTO
{
    public  class CountryAddRequest
    {
        public string? CountryName { get; set; }

        public Country? ToCountry()
        {
            return new Country() { CountryName = this.CountryName };
        }
    }
}
