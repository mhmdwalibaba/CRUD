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

        public Task<PersonResponse?> GetPersonByPersonID(Guid? perosnID)
        {
            throw new NotImplementedException();
        }
    }
}
