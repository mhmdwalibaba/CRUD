using Xunit;
using ServiceContracts.DTO;
using ServiceContracts;
using Services;
using FluentAssertions;
using AutoFixture;
namespace CRUDTest
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly IFixture _fixture;

        public CountriesServiceTest()
        {
            _countriesService = new CountriesService();
            _fixture = new Fixture();
        }

        #region CountryAddRequest
        //When we suply CountryAddRequest null,should be retrun Argument null Exeption
        public void CountryAdd_NullAddReqest()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = null;

            
            //act
            Action action = () =>
            {
                
                _countriesService.AddCountry(countryAddRequest);
            };
            //Assert
            action.Should().Throw<ArgumentNullException>();
        }
        //When we suply CountryName null,should be retrun Argument  Exeption
        public void CountryAdd_NullPersonName()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = _fixture.Build<CountryAddRequest>()
                .With(temp => temp.CountryName,null as string)
                .Create();


            //act
            Action action = () =>
            {

                _countriesService.AddCountry(countryAddRequest);
            };
            //Assert
            action.Should().Throw<ArgumentException>();
        }
        //When the CountryName is duplicate, it should throw ArgumentException
        public void CountryAdd_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest? first_country_add = _fixture
                .Build<CountryAddRequest>()
                .With(temp => temp.CountryName, "Iran")
                .Create();
            CountryAddRequest? second_country_add = _fixture
                .Build<CountryAddRequest>()
                .With(temp => temp.CountryName, "Iran")
                .Create();
            //Act
            CountryResponse? first_country_Response = _countriesService.AddCountry(first_country_add);
            
            Action action = () =>
            {
                _countriesService.AddCountry(second_country_add);
            };
            //Assert
            action.Should().Throw<ArgumentException>();

        }
        //When you supply proper country name, it should insert (add) the country to the existing list of countries
        public void CountryAdd_FullCountryDetails()
        {
            //Arrange
            CountryAddRequest? country_from_add_country = _fixture
                .Create<CountryAddRequest>();

            //Act
            CountryResponse country_response = _countriesService.AddCountry(country_from_add_country);

            //Assert
            country_from_add_country.count.Should().NotBe(Guid.Empty);
            country_from_add_country.Should().BeEquivalentTo(country_response);


        }
        #endregion
    }
}