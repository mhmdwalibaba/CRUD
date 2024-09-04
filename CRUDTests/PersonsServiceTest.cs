using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
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
namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly Fixture _fixture;
        private readonly Mock<IPersonsRepository> _personsRepostoryMock;
        private readonly IPersonsRepository _personsRepository;
        public PersonsServiceTest()
        {
            _personsRepostoryMock=new Mock<IPersonsRepository>();
            _personsRepository = _personsRepostoryMock.Object;

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

            _personsRepostoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);
            
            //Act
            PersonResponse? person_response_from_add = await _personsService.AddPerson(personAddRequest);

            person_response_expected.PersonID = person_response_from_add.PersonID;
            //Assert
            person_response_from_add.PersonID.Should().NotBe(Guid.Empty);
            person_response_from_add.Should().Be(person_response_expected);
        }

        #endregion
    }
}
