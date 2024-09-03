using Entits;
using System.Collections.Generic;

namespace IRepositoryContracts
{
    public interface  ICountriesRepository
    {
        /// <summary>
        /// add a new Country object into the dataStore
        /// </summary>
        /// <param name="country">country object to add</param>
        /// <returns>Return the Country object after adding into the datastore</returns>
        Task<Country> AddCountry(Country country);
        /// <summary>
        /// Retrun of list Country in data Store
        /// </summary>
        /// <returns>return list of Country</returns>
        Task<List<Country>> GetAllCountry();
        /// <summary>
        /// Retruns Country object based given countryid:otherwise null
        /// </summary>
        /// <param name="countryId">Guid countryID</param>
        /// <returns>return country or null</returns>
        Task<Country?> GetCountryByCountryId(Guid countryId);
        /// <summary>
        /// Returns Country object based on given countryid:otherWise null
        /// </summary>
        /// <param name="countryName">countryname to search</param>
        /// <returns>return country or null</returns>
        Task<Country?> GetCountryByCountryName(string countryName);

    }
}