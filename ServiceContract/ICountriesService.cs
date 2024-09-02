using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;
namespace ServiceContracts
{
    public  interface ICountriesService
    {
        /// <summary>
        ///  Adds a country object to the list of countries 
        /// </summary>
        /// <param name="countryAddRequest">country object to Add</param>
        /// <returns>returns the country object after adding it (including newly generated country id)</returns>
        public CountryResponse? AddCountry(CountryAddRequest countryAddRequest);
        /// <summary>
        /// Returns a country object based on the given country id
        /// </summary>
        /// <param name="countryId">CountryID (guid) to Search</param>
        /// <returns>maching country as country object Response</returns>
        public CountryResponse? GetCountryByCountryID(Guid countryId);
        /// <summary>
        /// Return all country of country list
        /// </summary>
        /// <returns>Retrun all countries or null if it is not exit </returns>
        public List<CountryResponse?> GetAllCountries();

    }
}
