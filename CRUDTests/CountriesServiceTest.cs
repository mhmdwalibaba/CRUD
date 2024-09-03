using System;
using Xunit;
using ServiceContracts.DTO;
using ServiceContracts;
using Services;
using FluentAssertions;
using AutoFixture;
using Services;
using Entits;
using EntityFrameworkCoreMock;
using Moq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CRUDTest
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly IFixture _fixture;

        public CountriesServiceTest()
        {
            var countriesInitialData = new List<Country>();
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>
                (new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            ApplicationDbContext dbContext= dbContextMock.Object;

            dbContextMock.CreateDbSetMock(temp => temp.countries, countriesInitialData);

            _countriesService = new CountriesService(dbContext);
            _fixture = new Fixture();
        }

        #region CountryAddRequest
        [Fact]
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
        [Fact]
        //When we suply CountryName null,should be retrun Argument  Exeption
        public void CountryAdd_NullPersonName()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = _fixture.Build<CountryAddRequest>()
                .With(temp => temp.CountryName, null as string)
                .Create();


            //act
            Action action = () =>
            {

                _countriesService.AddCountry(countryAddRequest);
            };
            //Assert
            action.Should().Throw<ArgumentException>();
        }
        [Fact]
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
        [Fact]
        //When you supply proper country name, it should insert (add) the country to the existing list of countries
        public void CountryAdd_FullCountryDetails()
        {
            //Arrange
            CountryAddRequest? country = _fixture
                .Create<CountryAddRequest>();
            Country? country_from_add_country = country.ToCountry();
            CountryResponse? country_from_add_Response = country_from_add_country.ToCountryResponse();
            //Act
            CountryResponse? country_response = _countriesService.AddCountry(country);

            country_from_add_country.CountryID = country_response.CountryID;
            country_from_add_Response.CountryID = country_response.CountryID;

            //Assert
            country_response.CountryID.Should().NotBe(Guid.Empty);
            country_from_add_Response.Should().BeEquivalentTo(country_response);


        }
        #endregion
    }
}