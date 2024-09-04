using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entits;
using IRepositoryContracts;



namespace Repository
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly ApplicationDbContext _db;

        public PersonsRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Person> AddPerson(Person person)
        {
            _db.persons.Add(person);
            await _db.SaveChangesAsync();
            return person;
        }

        public Task<List<Person>?> GetAllPerson()
        {
            throw new NotImplementedException();
        }

        public Task<Person?> GetPersonByPersonID(Guid? personID)
        {
            throw new NotImplementedException();
        }
    }
}
