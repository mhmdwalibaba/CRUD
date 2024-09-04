using System;
using System.Collections.Generic;
using Entits;
using ServiceContracts.DTO;
using ServiceContracts;
using Services;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Moq;
using AutoFixture;
using FluentAssertions;
using IRepositoryContracts;
using System.Linq;

namespace CRUDTest
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly Mock<ICountriesRepository> _countriesRepositoryMock;
        private readonly ICountriesRepository _countriesRepository;


        private readonly IFixture _fixture;

        public CountriesServiceTest()
        {

            _countriesRepositoryMock = new Mock<ICountriesRepository>();

            _countriesRepository = _countriesRepositoryMock.Object;

            _countriesService = new CountriesService(_countriesRepository);


            _fixture = new Fixture();
            


        }

        #region CountryAddRequest
        [Fact]
        //When we suply CountryAddRequest null,should be retrun Argument null Exeption
        public async Task CountryAdd_NullAddReqest()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = null;

           
            Country country = _fixture.Build<Country>()
                 .With(temp => temp.Persons, null as List<Person>)
                 .Create();

            _countriesRepositoryMock
             .Setup(temp => temp.AddCountry(It.IsAny<Country>()))
             .ReturnsAsync(country);

            //act
            //Act
            var action = async () =>
            {
                await _countriesService.AddCountry(countryAddRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }


        [Fact]
        //When we suply CountryName null,should be retrun Argument  Exeption
        public async Task CountryAdd_NullPersonName()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = _fixture.Build<CountryAddRequest>()
                .With(temp => temp.CountryName, null as string)
                .Create();

            Country? country = _fixture
                .Build<Country>()
                .With(temp => temp.Persons, null as List<Person>)
                .Create();
                

            _countriesRepositoryMock.Setup(temp => temp.AddCountry(It.IsAny<Country>()))
                .ReturnsAsync(country);


            //act
            var action = async () =>
             {

                 await _countriesService.AddCountry(countryAddRequest);
             };
            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }


        [Fact]
        //When the CountryName is duplicate, it should throw ArgumentException
        public async Task CountryAdd_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest? first_country_request = _fixture
                .Build<CountryAddRequest>()
                .With(temp => temp.CountryName, "Iran")
                .Create();
            CountryAddRequest? second_country_request = _fixture
                .Build<CountryAddRequest>()
                .With(temp => temp.CountryName, "Iran")
                .Create();

            Country first_country = first_country_request.ToCountry();
            Country second_country = second_country_request.ToCountry();

            _countriesRepositoryMock
            .Setup(temp => temp.AddCountry(It.IsAny<Country>()))
            .ReturnsAsync(first_country);

            //Return null when GetCountryByCountryName is called
            _countriesRepositoryMock
             .Setup(temp => temp.GetCountryByCountryName(It.IsAny<string>()))
             .ReturnsAsync(null as Country);


            //Act
            CountryResponse? first_country_Response = await _countriesService.AddCountry(first_country_request);

            var action = async () =>
             {
                //Return first country when GetCountryByCountryName is called
                _countriesRepositoryMock.Setup(temp => temp.AddCountry(It.IsAny<Country>())).ReturnsAsync(first_country);

                 _countriesRepositoryMock.Setup(temp => temp.GetCountryByCountryName(It.IsAny<string>())).ReturnsAsync(first_country);

                 await _countriesService.AddCountry(second_country_request);
             };
            //Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }


        [Fact]
        //When you supply proper country name, it should insert (add) the country to the existing list of countries
        public async Task CountryAdd_FullCountryDetails()
        {
            //Arrange
            CountryAddRequest? country = _fixture
                .Create<CountryAddRequest>();
            Country? country_from_add_country = country.ToCountry();
            CountryResponse? country_from_add_Response = country_from_add_country.ToCountryResponse();

            //Send Return Data for When excuting Repositroy
            _countriesRepositoryMock
                .Setup(temp => temp.AddCountry(It.IsAny<Country>()))
                .ReturnsAsync(country_from_add_country);
            _countriesRepositoryMock
                .Setup(temp => temp.GetCountryByCountryName(It.IsAny<string>()))
                .ReturnsAsync(null as Country);

            //Act
            CountryResponse? country_response = await _countriesService.AddCountry(country);

            country_from_add_country.CountryID = country_response.CountryID;
            country_from_add_Response.CountryID = country_response.CountryID;

            //Assert
            country_response.CountryID.Should().NotBe(Guid.Empty);
            country_from_add_Response.Should().BeEquivalentTo(country_response);


        }

        #endregion

        #region GetAllCountry

        [Fact]
        //The list of countries should be empty by default (before adding any countries)
        public async Task GetAllCountry_ShouldReturnEmptyList()
        {
            //Arrange
            List<Country> countriesList_Empty = new List<Country>();
            _countriesRepositoryMock.Setup(temp =>temp.GetAllCountry()).ReturnsAsync(countriesList_Empty);

            //Act
            List<CountryResponse> actual_country_response_list = await _countriesService.GetAllCountries();

            //Assert
            actual_country_response_list.Should().BeEmpty();
        }
        [Fact]
        public async Task GetAllCountry_ShouldHaveFewCountries()
        {
            List<Country> country_list = new List<Country>()
            {
                _fixture.Build<Country>().With(temp => temp.Persons, null as List<Person>).Create(),
                _fixture.Build<Country>().With(temp => temp.Persons, null as List<Person>).Create(),
            };
            List<CountryResponse?> country_list_Response = country_list.Select(temp => temp.ToCountryResponse()).ToList();


            _countriesRepositoryMock
                .Setup(temp => temp.GetAllCountry())
                .ReturnsAsync(country_list);

            //Act
            List<CountryResponse?> actual_country_Respones = await _countriesService.GetAllCountries();

            //Assert
            actual_country_Respones.Should().BeEquivalentTo(country_list_Response);
        }




        #endregion

        #region GetCountryByCountryId

        [Fact]
        //If we supply null as CountryID, it should return null as CountryResponse
        public async Task GetCountryByCountryId_NullCountryID()
        {
            Guid? countryId = null;

           //Act
            CountryResponse? countryResposne= await  _countriesService.GetCountryByCountryID(countryId);

            //Assert
            countryResposne.Should().BeNull();
            

        }
        //if we supply valid Coutnryid,it should Return coutnry with given on Countryid
        public async Task GetCountryByCountryId_ValidCountryId()
        {
            Country country = _fixture.Create<Country>();

            CountryResponse? countryResponse = country.ToCountryResponse();

            //Act
            CountryResponse? actual_country_response_get = await _countriesService.GetCountryByCountryID(country.CountryID);

            //Assert
            actual_country_response_get.Should().Be(countryResponse);
            
        }

        #endregion
    }
}