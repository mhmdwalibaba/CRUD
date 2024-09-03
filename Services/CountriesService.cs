using Entits;
using ServiceContracts;
using ServiceContracts.DTO;
using IRepositoryContracts;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly ICountriesRepository _countriesRepository;

        public CountriesService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }
        public async Task<CountryResponse?> AddCountry(CountryAddRequest countryAddRequest)
        {
            //Check if "countryAddRequest" is not null.
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            //Validate all properties of "countryAddRequest"
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            if (await _countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName)!=null)
            {
                throw new ArgumentException();
            }
            //Convert "countryAddRequest" from "CountryAddRequest" type to "Country".
            Country? country = countryAddRequest.ToCountry();

            //Generate a new CountryID
            country.CountryID = Guid.NewGuid();

            //Then add it into dataBase
            await _countriesRepository.AddCountry(country);
            
            //Return CountryResponse object with generated CountryID
            CountryResponse? countryResponse = country.ToCountryResponse();
            return countryResponse;
        }

        public async Task<List<CountryResponse?>> GetAllCountries()
        {
            List<Country> countries = await _countriesRepository.GetAllCountry();

            List<CountryResponse?> countryResponses = countries.Select(temp => temp.ToCountryResponse()).ToList();

            return countryResponses;
        }

        public async Task<CountryResponse?> GetCountryByCountryID(Guid? countryId)
        {
            if (countryId == null)
                return null;
            Country? country = await _countriesRepository.GetCountryByCountryId(countryId.Value);
            return country.ToCountryResponse();

        }
    }
}