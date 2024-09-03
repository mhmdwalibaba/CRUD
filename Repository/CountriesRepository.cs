using Entits;
using IRepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly ApplicationDbContext _db;
        public CountriesRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Country> AddCountry(Country country)
        {
            _db.countries.Add(country);
            await _db.SaveChangesAsync();
            return country;
        }

        public async Task<List<Country>> GetAllCountry()
        {
            return await _db.countries.ToListAsync();
        }

        public async Task<Country?> GetCountryByCountryId(Guid countryId)
        {
            Country? coutnry =await  _db.countries.FirstOrDefaultAsync(temp => temp.CountryID == countryId);
            return coutnry;
        }

        public async Task<Country?> GetCountryByCountryName(string countryName)
        {
            Country? coutnry = await _db.countries.FirstOrDefaultAsync(temp => temp.CountryName == countryName);
            return coutnry;
        }
    }
}