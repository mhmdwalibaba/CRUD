﻿using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using FluentAssertions;
using Services;
using ServiceContracts.DTO;
using Xunit;
using Entits;
using IRepositoryContracts;
using ServiceContracts.Enums;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly Fixture _fixture;
        private readonly Mock<IPersonsRepository> _personsRepositoryMock;
        private readonly IPersonsRepository _personsRepository;
        public PersonsServiceTest()
        {
            _personsRepositoryMock = new Mock<IPersonsRepository>();
            _personsRepository = _personsRepositoryMock.Object;

            _personsService = new PersonsService(_personsRepository);


            _fixture = new Fixture();
        }

        #region AddPersons
        [Fact]
        //supply perosn Addrequest ,should be throw ArgumentNullExepetion
        public async Task AddPerson_NullAddRequest()
        {
            PersonAddRequest? personAddRequest = null;

            //Act
            var action = async () =>
            {
                await _personsService.AddPerson(personAddRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }
        [Fact]
        //if supply PersonName is Null,should be throw ArgumentExeption
        public async Task AddPerson_NullPersonName()
        {
            PersonAddRequest personAddRequest = _fixture
                .Build<PersonAddRequest>()
                .With(temp => temp.PersonName, null as string)
                .Create();

            //Act
            var action = async () =>
              {
                  await _personsService.AddPerson(personAddRequest);
              };
            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        //if We supply duplicate PersonAddRequest , shouldBe Throw ArgumentExeption
        public async Task AddPerson_DatailsPerson_ValidPerson()
        {
            //Arrange
            var personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "MohammadFarkhani@gmail.com")
                .Create();

            Person? person = personAddRequest.ToPerson();
            PersonResponse? person_response_expected = person.ToPersonResponse();

            _personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);

            //Act
            PersonResponse? person_response_from_add = await _personsService.AddPerson(personAddRequest);

            person_response_expected.PersonID = person_response_from_add.PersonID;
            //Assert
            person_response_from_add.PersonID.Should().NotBe(Guid.Empty);
            person_response_from_add.Should().Be(person_response_expected);
        }

        #endregion

        #region GetAllPerson

        [Fact]
        //if List of Empty,should Return Empty
        public async Task GetAllPerson_EmptyList()
        {
            //Arrange
            List<Person> persons_list_Empty = new List<Person>();

            _personsRepositoryMock
                .Setup(temp => temp.GetAllPerson())
                .ReturnsAsync(persons_list_Empty);
            //Act
            List<PersonResponse?> personResponses = await _personsService.GetAllPersons();
            //Assert
            personResponses.Should().BeEmpty();
        }
        [Fact]
        //First, we will add few persons; and then when we call GetAllPersons(), it should return the same persons that were added
        public async Task GetAllPersons_WithFewPersons_ToBeSuccessfull()
        {
            //Arrange
            List<Person> persons = new List<Person>() {

                _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_1@example.com")
                .With(temp => temp.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
                .With(temp => temp.Email, "someone_2@example.com")
                .With(temp => temp.Country, null as Country)
                .Create(),

                _fixture.Build<Person>()
               .With(temp => temp.Email, "someone_3@example.com")
               .With(temp => temp.Country, null as Country)
               .Create()
             };

            _personsRepositoryMock
               .Setup(temp => temp.GetAllPerson())
               .ReturnsAsync(persons);

            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();

            //Act
            List<PersonResponse>? persons_list_get = await _personsService.GetAllPersons();

            //Assert
            persons_list_get.Should().BeEquivalentTo(person_response_list_expected);

        }
        #endregion

        #region GetPersonByPersonID
        [Fact]
        //When We supply null PersonId ,ShouldBe Return Null
        public async Task GetPersonByPersonID_NullPersonId()
        {
            Guid? personID = null;

            //Act
            PersonResponse? personResponse = await _personsService.GetPersonByPersonID(personID);

            //Assert
            personResponse.Should().BeNull();
        }
        [Fact]
        // //If we supply a valid person id, it should return the valid person details as PersonResponse object
        public async Task GetPersonByPersonID_ValidPersonID_toBeSuccessFull()
        {
            //Arrange
            Person person = _fixture
                .Build<Person>()
                .With(temp => temp.Email, "email@sample.com")
                .With(temp => temp.Country, null as Country)
                .Create();

            PersonResponse person_response_expected = person.ToPersonResponse();

            _personsRepositoryMock
                .Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
                .ReturnsAsync(person);


            //Act
            PersonResponse? person_response_from_get = await _personsService.GetPersonByPersonID(person.PersonID);


            //Assert
            person_response_from_get.Should().Be(person_response_expected);
        }


        #endregion

        #region GetFiltredPerson

        [Fact]
        //If the search text is empty and search by is "PersonName", it should return all persons
        public async Task GetFiltredPerson_EmptySearchText()
        {
            //Arrange
            List<Person> persons = new List<Person>() {
             _fixture.Build<Person>()
             .With(temp => temp.Email, "someone_1@example.com")
             .With(temp => temp.Country, null as Country)
             .Create(),

            _fixture.Build<Person>()
            .With(temp => temp.Email, "someone_2@example.com")
            .With(temp => temp.Country, null as Country)
            .Create(),

             _fixture.Build<Person>()
             .With(temp => temp.Email, "someone_3@example.com")
             .With(temp => temp.Country, null as Country)
             .Create()
             };

            List<PersonResponse?> person_list_from_exepted = persons.Select(temp => temp.ToPersonResponse()).ToList();

            _personsRepositoryMock
                .Setup(temp => temp.GetFiltredPerson(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(persons);


            //Act
            List<PersonResponse?> person_list_from_search = await _personsService.GetFiltredPerson(nameof(Person.PersonName), null);

            //Assert
            person_list_from_search.Should().BeEquivalentTo(person_list_from_exepted);
        }
        [Fact]
        //First we will add few persons; and then we will search based on person name with some search string. It should return the matching persons
        public async Task GetFiltredPerson_ByPersonName()
        {
            //Arrange
            List<Person> persons = new List<Person>() {
             _fixture.Build<Person>()
             .With(temp => temp.Email, "someone_1@example.com")
             .With(temp => temp.Country, null as Country)
             .Create(),

            _fixture.Build<Person>()
            .With(temp => temp.Email, "someone_2@example.com")
            .With(temp => temp.Country, null as Country)
            .Create(),

             _fixture.Build<Person>()
             .With(temp => temp.Email, "someone_3@example.com")
             .With(temp => temp.Country, null as Country)
             .Create()
             };

            List<PersonResponse?> person_list_from_exepted = persons.Select(temp => temp.ToPersonResponse()).ToList();

            _personsRepositoryMock
                .Setup(temp => temp.GetFiltredPerson(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(persons);


            //Act
            List<PersonResponse?> person_list_from_search = await _personsService.GetFiltredPerson(nameof(Person.PersonName), "sa");

            //Assert
            person_list_from_search.Should().BeEquivalentTo(person_list_from_exepted);
        }

        #endregion

        #region GetSortedPerson

        //When we sort based on PersonName in DESC, it should return persons list in descending on PersonName
        [Fact]
        public async Task GetSortedPersons_ToBeSuccessful()
        {
            //Arrange
            List<Person> persons = new List<Person>() {
             _fixture.Build<Person>()
             .With(temp => temp.Email, "someone_1@example.com")
             .With(temp => temp.Country, null as Country)
             .Create(),

             _fixture.Build<Person>()
             .With(temp => temp.Email, "someone_2@example.com")
             .With(temp => temp.Country, null as Country)
             .Create(),

             _fixture.Build<Person>()
             .With(temp => temp.Email, "someone_3@example.com")
             .With(temp => temp.Country, null as Country)
             .Create()

             };

            List<PersonResponse?> person_list_exepted = persons.Select(temp => temp.ToPersonResponse()).ToList();

            _personsRepositoryMock
            .Setup(temp => temp.GetAllPerson())
            .ReturnsAsync(persons);

            //Act
            List<PersonResponse> allPersons = await _personsService.GetAllPersons();


            List<PersonResponse> persons_list_from_sort = await _personsService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            persons_list_from_sort.Should().BeInDescendingOrder(temp => temp.PersonName);
        }
            #endregion

       #region PersonUpdate
       [Fact]
        //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        public async Task PersonUpdate_nullPersonUpdate_ToBeArgumentExeption()
        {
                PersonUpdateRequest? person_update_request = null;

                //Act
                var action = async () =>
                {
                    await _personsService.UpdatePerson(person_update_request);
                };

                //Assert
                await action.Should().ThrowAsync<ArgumentNullException>();
        }
        [Fact]
       // when Person name is null, it should throw ArgumentException
        public async Task PersonUpdate_PersonNameNull_ToBeArgumentException()
        {
           Person person = _fixture.Build<Person>()
                 .With(temp => temp.PersonName, null as string)
                 .With(temp => temp.Email, "someone@example.com")
                 .With(temp => temp.Country, null as Country)
                 .With(temp => temp.Gender, "Male")
                 .Create();

            PersonResponse person_response_from_add = person.ToPersonResponse();

            PersonUpdateRequest personUpdateRequest = person_response_from_add.ToPersonUpdateRequest();

            var action = async () =>
             {
                 await _personsService.UpdatePerson(personUpdateRequest);
             };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When we supply invalid person id, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_InvalidPersonID_ToBeArgumentException()
        {
            //Arrange
            PersonUpdateRequest? person_update_request = _fixture.Build<PersonUpdateRequest>()
             .Create();

            //Act
            Func<Task> action = async () =>
            {
                await _personsService.UpdatePerson(person_update_request);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }
        [Fact]
        //When we supply valid person id, it should update person
        public async Task UpdatePerson_PersonFullDetails_ToBeSuccessful()
        {
            Person person = _fixture.Build<Person>()
                .With(temp => temp.Email, "someone@example.com")
                .With(temp => temp.Country, null as Country)
                .With(temp => temp.Gender, "Male")
                .Create();

            PersonResponse person_response_expected = person.ToPersonResponse();

            PersonUpdateRequest person_update_request = person_response_expected.ToPersonUpdateRequest();


            _personsRepositoryMock
                .Setup(temp => temp.UpdatePerosn(It.IsAny<Person>()))
                .ReturnsAsync(person);


            _personsRepositoryMock
                .Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
                .ReturnsAsync(person);

            //Act
            PersonResponse person_response_from_update = await _personsService.UpdatePerson(person_update_request);

            //Assert
            person_response_from_update.Should().Be(person_response_expected);

        }
        //When we supply invalid person id, it should return flase
        [Fact]
        public async Task DeletePerson_InvalidPersonID_toBeFalse()
        {
            bool isDeleted = await _personsService.DeletePerson(Guid.NewGuid());

            isDeleted.Should().BeFalse();
        }
        //When we supply valid Person Id,it should Return True
        [Fact]
        
        public async Task DeletPerson_ValidPersonID_ToBeTrue()
        {
            Person? person = _fixture.Build<Person>()
                    .With(temp => temp.Email, "someone@example.com")
                    .With(temp => temp.Country, null as Country)
                    .With(temp => temp.Gender, "Female")
                    .Create();

            _personsRepositoryMock
              .Setup(temp => temp.DeletePersonByPersonID(It.IsAny<Guid>()))
              .ReturnsAsync(true);

            _personsRepositoryMock
             .Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
             .ReturnsAsync(person);

            //Act
            bool isDeleted = await _personsService.DeletePerson(person.PersonID);

            //Assert
            isDeleted.Should().BeTrue();
        }
        #endregion

    }
}
