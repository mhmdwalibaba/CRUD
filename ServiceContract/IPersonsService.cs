using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsService
    {
        /// <summary>
        /// Addds a new person into the list of persons
        /// </summary>
        /// <param name="personAddRequest">Person to Add</param>
        /// <returns>Returns the same person details, along with newly generated PersonID</returns>
        Task<PersonResponse?> AddPerson(PersonAddRequest personAddRequest);

        /// <summary>
        /// Return all person into list
        /// </summary>
        /// <returns>List of person</returns>
        Task<List<PersonResponse>?> GetAllPersons();

        /// <summary>
        /// Get Person based on given  PersonID: otherWise null
        /// </summary>
        /// <param name="perosnID">Guid PersonID</param>
        /// <returns>Returns matching person object</returns>
        Task<PersonResponse?> GetPersonByPersonID(Guid? perosnID);

        /// <summary>
        /// Returns all person objects that matches with the given search field and search string
        /// </summary>
        /// <param name="searchBy">Search field to search</param>
        /// <param name="searchString">Search string to search</param>
        /// <returns>Returns all matching persons based on the given search field and search string</returns>
        Task<List<PersonResponse>?> GetFiltredPerson(string? searchBy, string? searchString);

        /// <summary>
        /// Returns sorted list of persons
        /// </summary>
        /// <param name="allPersons">Represents list of persons to sort</param>
        /// <param name="sortBy">Name of the property (key), based on which the persons should be sorted</param>
        /// <param name="sortOrder">ASC or DESC</param>
        /// <returns>Returns sorted persons as PersonResponse list</returns>
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);

        /// <summary>
        /// update perosn and return resposne
        /// </summary>
        /// <param name="personUpdateRequest">object for updateRequest</param>
        /// <returns>retrun updated persons in data stpre</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        /// <summary>
        /// retrun true when object deleted success in Data store
        /// </summary>
        /// <param name="personID">Id Person</param>
        /// <returns>boolean true or false</returns>
        Task<bool> DeletePerson(Guid? personID);

    }
}
