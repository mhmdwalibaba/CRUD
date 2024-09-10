using Entits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        /// <summary>
        /// Returns all person objects based on the given expression
        /// </summary>
        /// <param name="predicate">LINQ expression to check</param>
        /// <returns>All matching persons with given condition</returns>
        Task<List<Person>> GetFiltredPerson(Expression<Func<Person, bool>> predicate);

        /// <summary>
        /// retuen updated person and stor in database
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Updated person</returns>
        Task<Person> UpdatePerosn(Person person);
        /// <summary>
        /// delete person in datastore with matching perosnid
        /// </summary>
        /// <param name="personId">personid</param>
        /// <returns>if person deleted return true else return false</returns>
        Task<bool> DeletePersonByPersonID(Guid personId);
    }
}
