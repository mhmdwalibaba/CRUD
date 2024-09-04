using Entits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepositoryContracts
{
    public interface IPersonsRepository
    {
        /// <summary>
        /// Add a new Person Object into the Data Store
        /// </summary>
        /// <param name="person">Person object ot Add</param>
        /// <returns>Person Object Added to data Store</returns>
        Task<Person> AddPerson(Person person);
        /// <summary>
        /// Return a person object based on given PersonId:OtherWise Return Null
        /// </summary>
        /// <param name="personID">PersonID</param>
        /// <returns>Return Person maching Personid or null</returns>
        Task<Person?> GetPersonByPersonID(Guid? personID);
        /// <summary>
        /// Get All Persons in DataStore
        /// </summary>
        /// <returns>List of AllPersons Stored in DataStore</returns>
        Task<List<Person>?> GetAllPerson();
    }
}
