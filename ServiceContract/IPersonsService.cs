﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;

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

    }
}
