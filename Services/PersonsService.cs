using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts;
using ServiceContracts.DTO;
using IRepositoryContracts;
using Entits;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly IPersonsRepository _personsRepository;
        public PersonsService(IPersonsRepository personsRepository)
        {
            _personsRepository = personsRepository;
        }

        public async Task<PersonResponse?> AddPerson(PersonAddRequest personAddRequest)
        {
            if(personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }
            if (personAddRequest.PersonName == null)
            {
                throw new ArgumentException(nameof(personAddRequest.PersonName));
            }

            //convert personAddRequest into Person type
            Person? person = personAddRequest.ToPerson();

            //genrate PersonID
            person.PersonID = Guid.NewGuid();

            //addPerson to the DataStore
             await _personsRepository.AddPerson(person);

            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>?> GetAllPersons()
        {
            List<Person>? persons = await _personsRepository.GetAllPerson();
            List<PersonResponse>? personResponses = persons.Select(temp => temp.ToPersonResponse()).ToList();

            return personResponses;
        }

        public async Task<List<PersonResponse>?> GetFiltredPerson(string? searchBy, string? searchString)
        {
            List<Person> persons = searchBy switch
            {
                nameof(PersonResponse.PersonName)=>
                await _personsRepository
                .GetFiltredPerson(temp => temp.PersonName.Contains(searchString)),

                nameof(PersonResponse.Email) =>
                await _personsRepository
                .GetFiltredPerson(temp => temp.Email.Contains(searchString)),

                nameof(PersonResponse.DateOfBirth) =>
                await _personsRepository.GetFiltredPerson(temp =>
                temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString)),


                nameof(PersonResponse.Gender) =>
                 await _personsRepository.GetFiltredPerson(temp =>
                 temp.Gender.Contains(searchString)),

                nameof(PersonResponse.CountryID) =>
                 await _personsRepository.GetFiltredPerson(temp =>
                 temp.Country.CountryName.Contains(searchString)),

                nameof(PersonResponse.Address) =>
                await _personsRepository.GetFiltredPerson(temp =>
                temp.Address.Contains(searchString)),

                _ => await _personsRepository.GetAllPerson()
            };
            return persons.Select(temp => temp.ToPersonResponse()).ToList();
        }

        public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            if (personID == null)
                return null;

            Person? person = await _personsRepository.GetPersonByPersonID(personID);

            return person.ToPersonResponse();
        }
    }
}
