using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        public CountryResponse? AddCountry(CountryAddRequest countryAddRequest)
        {
            //Check if "countryAddRequest" is not null.
            //Validate all properties of "countryAddRequest"
            //Convert "countryAddRequest" from "CountryAddRequest" type to "Country".
            //Generate a new CountryID
            //Then add it into List<Country>
            //Return CountryResponse object with generated CountryID
            throw new NotImplementedException();
        }

        public List<CountryResponse?> GetAllCountries()
        {
            throw new NotImplementedException();
        }

        public CountryResponse? GetCountryByCountryID(Guid countryId)
        {
            throw new NotImplementedException();
        }
    }
}