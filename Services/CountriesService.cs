using Entits;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly ApplicationDbContext _db;

        public CountriesService(ApplicationDbContext db)
        {
            _db = db;
        }
        public CountryResponse? AddCountry(CountryAddRequest countryAddRequest)
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

            if (_db.countries.Count(temp => temp.CountryName == countryAddRequest.CountryName) != 0)
            {
                throw new ArgumentException();
            }
            //Convert "countryAddRequest" from "CountryAddRequest" type to "Country".
            Country? country = countryAddRequest.ToCountry();

            //Generate a new CountryID
            country.CountryID = Guid.NewGuid();

            //Then add it into dataBase
            _db.countries.Add(country);
            _db.SaveChanges();
            //Return CountryResponse object with generated CountryID
            CountryResponse? countryResponse = country.ToCountryResponse();
            return countryResponse;
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