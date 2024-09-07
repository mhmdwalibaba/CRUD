using ServiceContracts;
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
using ServiceContracts.Enum;

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
            #endregion

            persons_list_from_sort.Should().BeInDescendingOrder(temp => temp.PersonName);
        }
    }
}
