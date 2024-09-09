using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entits;
using IRepositoryContracts;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Person>?> GetAllPerson()
        {
            List<Person> persons = await _db.persons.Include("Country").ToListAsync();

            return persons;
        }

        public async Task<List<Person>> GetFiltredPerson(Expression<Func<Person, bool>> predicate)
        {
           return await _db.persons.Include("Country").Where(predicate).ToListAsync();
        }

        public async Task<Person?> GetPersonByPersonID(Guid? personID)
        {
            return await _db.persons.Include("Country").FirstOrDefaultAsync(temp => temp.PersonID == personID);
        }

        public async Task<Person> UpdatePerosn(Person person)
        {
            Person? matchingPerson = await _db.persons.FirstOrDefaultAsync(temp => temp.PersonID == person.PersonID);

            if (matchingPerson == null)
                return null;

            matchingPerson.PersonName=person.PersonName;
            matchingPerson.DateOfBirth = person.DateOfBirth;
            matchingPerson.CountryID = person.CountryID;
            matchingPerson.Email = person.Email;
            matchingPerson.Address = person.Address;
            matchingPerson.Gender = person.Gender;
            matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;

            int countUpdated = await _db.SaveChangesAsync();

            return matchingPerson;
        }
    }
}
